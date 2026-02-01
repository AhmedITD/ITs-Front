using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Api.Common;
using RentARide.Application.DTOs.Requests.QiCard;
using RentARide.Application.DTOs.requests.Invoice;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.responses.Invoice;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Api.Controllers;

[Route("rentals")]
public class InvoiceController(IInvoiceService invoiceService) : BaseController
{
    [Authorize]
    [HttpPost("invoice")]
    public async Task<ActionResult<ApiResponse<InvoiceResponse>>> CreateInvoice(InvoiceRequest request, CancellationToken cancellationToken)
    {
        request.BrowserInfo = BuildBrowserInfoFromContext(HttpContext);
        var result = await invoiceService.CreateInvoice(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    private static BrowserInfo BuildBrowserInfoFromContext(HttpContext context)
    {
        var request = context.Request;
        return new BrowserInfo
        {
            BrowserUserAgent = request.Headers["User-Agent"].FirstOrDefault() ?? "Mozilla/5.0",
            BrowserAcceptHeader = request.Headers["Accept"].FirstOrDefault() ?? "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            BrowserLanguage = request.Headers["Accept-Language"].FirstOrDefault() ?? "en-US",
            BrowserIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString(),
            BrowserJavaEnabled = false,
        };
    }
}