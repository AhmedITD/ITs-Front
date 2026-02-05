using RentARide.Domain.Enums;
using RentARide.Domain.Interfaces;

namespace RentARide.Domain.Entities;

public class Invoice : ISoftDeletable , IAuditable
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? AmenityIdsJson { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "IQD";
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public string? QiPaymentId { get; set; }
    public string? SuperQiPaymentId { get; set; }
    public string? SuperQiPaymentRequestId { get; set; }
    public DateTime? PaidAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public int? UpdatedBy { get; set; }

    public User User { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
}