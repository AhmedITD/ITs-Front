using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentARide.Application.Common;
using RentARide.Application.DTOs.requests.Invoice;
using RentARide.Application.DTOs.Requests.QiCard;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.responses.Invoice;
using RentARide.Application.DTOs.Responses.QiCard;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;

namespace RentARide.Application.Services;

public class InvoiceService(
    IRentARideDbContext dbContext,
    ICurrentUser currentUser,
    IQiCardService qiCardService,
    IPublicHolidayService publicHolidayService,
    IConfiguration configuration
) : IInvoiceService
{
    public async Task<ApiResponse<InvoiceResponse>> CreateInvoice(InvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await dbContext.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (vehicle == null)
            return ApiResponse<InvoiceResponse>.ErrorResponse("Vehicle not found.");
        if (vehicle.Status != VehicleStatus.Available)
            return ApiResponse<InvoiceResponse>.ErrorResponse("Vehicle is not available.");

        var startUtc = request.StartDate.ToUtc();
        var endUtc = request.EndDate.ToUtc();
        var hasOverlap = await dbContext.Rentals
            .AnyAsync(r =>
                r.VehicleId == request.VehicleId &&
                r.Status == RentalStatus.Active &&
                startUtc < r.EndDate &&
                endUtc > r.StartDate,
                cancellationToken);
        if (hasOverlap)
            return ApiResponse<InvoiceResponse>.ErrorResponse("Vehicle is already rented for the selected dates.");

        var days = (int)Math.Ceiling((endUtc - startUtc).TotalDays);
        if (days < 1) days = 1;

        var validAmenityIds = request.AmenityIds.Where(id => id > 0).Distinct().ToList();
        var amenities = validAmenityIds.Count > 0
            ? await dbContext.Amenities
                .Where(a => validAmenityIds.Contains(a.Id))
                .ToListAsync(cancellationToken)
            : new List<Amenity>();

        if (validAmenityIds.Count > 0 && amenities.Count != validAmenityIds.Count)
        {
            var foundIds = amenities.Select(a => a.Id).ToHashSet();
            var missing = string.Join(", ", validAmenityIds.Where(id => !foundIds.Contains(id)));
            return ApiResponse<InvoiceResponse>.ErrorResponse($"Amenity id(s) not found: {missing}.");
        }

        var amenityPrice = amenities.Sum(a => a.Price);
        var baseTotal = (days * vehicle.DailyPrice) + amenityPrice;
        var holidaySurcharge = 0m;
        if (await publicHolidayService.IsPublicHolidayAsync(startUtc, cancellationToken))
            holidaySurcharge = baseTotal * 0.10m;
        var totalPrice = baseTotal + holidaySurcharge;

        var invoice = new Invoice()
        {
            Id = Guid.NewGuid(),
            UserId = currentUser.Id,
            VehicleId = request.VehicleId,
            StartDate = startUtc,
            EndDate = endUtc,
            AmenityIdsJson = validAmenityIds.Count > 0 ? JsonSerializer.Serialize(validAmenityIds) : null,
            TotalAmount = totalPrice,
            Currency = "IQD",
            Status = InvoiceStatus.Draft,
        };

        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);

        var user = await dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == currentUser.Id)
            .Select(u => new { u.FirstName, u.LastName, u.Email })
            .FirstOrDefaultAsync(cancellationToken);
        var customerInfo = user != null
            ? new CustomerInfo { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email }
            : new CustomerInfo { FirstName = "Customer" };

        var appUrl = configuration["APP_URL"];
        var qiCardPaymentRequest = new QiCardPaymentRequest()
        {
            RequestId = invoice.Id.ToString(),
            Amount = invoice.TotalAmount,
            Currency = "IQD",
            FinishPaymentUrl = request.FinishUrl,
            NotificationUrl = $"{appUrl}/api/payments/webhook",
            CustomerInfo = customerInfo,
            BrowserInfo = request.BrowserInfo,
        };
        QiCardResponse qiResponse = await qiCardService.InitiatePaymentAsync(qiCardPaymentRequest, cancellationToken);

        if (!qiResponse.Success)
            return ApiResponse<InvoiceResponse>.ErrorResponse(qiResponse.Error ?? "Payment initiation failed.");

        invoice.QiPaymentId = qiResponse.PaymentId;
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<InvoiceResponse>.SuccessResponse(new InvoiceResponse
        {
            InvoiceId = invoice.Id,
            PaymentUrl = qiResponse.PaymentUrl ?? string.Empty,
        });
    }

    public async Task<ApiResponse<bool>> MarkAsPaid(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId, cancellationToken);
        if (invoice == null)
            return ApiResponse<bool>.ErrorResponse("Invoice not found.");
        if (invoice.Status == InvoiceStatus.Paid)
            return ApiResponse<bool>.SuccessResponse(true); // Idempotent
        invoice.Status = InvoiceStatus.Paid;
        invoice.PaidAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResponse(true);
    }

    public async Task CancelOtherPendingPaymentsForVehicle(Guid paidInvoiceId, CancellationToken cancellationToken = default)
    {
        var paidInvoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == paidInvoiceId, cancellationToken);
        if (paidInvoice == null) return;

        var startUtc = paidInvoice.StartDate;
        var endUtc = paidInvoice.EndDate;
        var vehicleId = paidInvoice.VehicleId;

        var otherPending = await dbContext.Invoices
            .Where(i =>
                i.VehicleId == vehicleId
                && i.Id != paidInvoiceId
                && (i.Status == InvoiceStatus.Draft || i.Status == InvoiceStatus.Sent)
                && startUtc < i.EndDate
                && endUtc > i.StartDate)
            .ToListAsync(cancellationToken);

        foreach (var inv in otherPending)
        {
            if (!string.IsNullOrWhiteSpace(inv.QiPaymentId))
                _ = await qiCardService.CancelPaymentAsync(inv.QiPaymentId);
            inv.Status = InvoiceStatus.Cancelled;
        }

        if (otherPending.Count > 0)
            await dbContext.SaveChangesAsync(cancellationToken);
    }
}