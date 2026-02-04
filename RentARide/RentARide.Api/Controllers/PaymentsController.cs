using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Api.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentsController(
    IQiCardService qiCardService,
    IInvoiceService invoiceService,
    IRentalService rentalService) : ControllerBase
{
    /// <summary>
    /// Webhook endpoint called by Qi when payment status changes.
    /// No auth - Qi identifies itself via signature in payload.
    /// </summary>
    [HttpPost("webhook")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(body))
            return BadRequest();

        // QiCard message verification: verify signature from header using raw body (https://developers-gate.qi.iq/docs/webhook-guide/message-verification)
        var signatureFromHeader = Request.Headers["X-Signature"].FirstOrDefault();
        if (!qiCardService.VerifyWebhookSignature(body, signatureFromHeader))
            return Unauthorized();

        Dictionary<string, object>? webhookData;
        try
        {
            webhookData = JsonElementToDictionary(JsonDocument.Parse(body).RootElement);
        }
        catch
        {
            return BadRequest();
        }

        var verifyResult = await qiCardService.ProcessWebhookAsync(webhookData);
        if (!verifyResult.Success)
            return BadRequest(verifyResult.Error);

        if (!TryGetRequestId(webhookData, out var requestId) || !Guid.TryParse(requestId, out var invoiceId))
            return Ok(); // Acknowledge to avoid retries; log in production

        if (!IsPaymentSuccess(webhookData))
            return Ok();

        var markResult = await invoiceService.MarkAsPaid(invoiceId, cancellationToken);
        if (!markResult.Success)
            return Ok(); // Idempotent or not found; already return 200 to avoid Qi retries

        _ = await rentalService.CreateRentalFromInvoice(invoiceId, cancellationToken);
        await invoiceService.CancelOtherPendingPaymentsForVehicle(invoiceId, cancellationToken);
        return Ok();
    }

    private static bool TryGetRequestId(Dictionary<string, object> data, out string? requestId)
    {
        requestId = GetString(data, "requestId") ?? GetString(data, "request_id");
        if (string.IsNullOrEmpty(requestId) && data.TryGetValue("data", out var dataObj) && dataObj is Dictionary<string, object> nested)
            requestId = GetString(nested, "requestId");
        return !string.IsNullOrEmpty(requestId);
    }

    private static string? GetString(Dictionary<string, object> data, string key)
    {
        if (!data.TryGetValue(key, out var v) || v == null) return null;
        if (v is string s) return s;
        if (v is JsonElement je) return je.GetString();
        return v.ToString();
    }

    private static bool IsPaymentSuccess(Dictionary<string, object> data)
    {
        var status = GetString(data, "status") ?? GetString(data, "paymentStatus") ?? GetString(data, "state");
        if (string.IsNullOrEmpty(status) && data.TryGetValue("data", out var dataObj) && dataObj is Dictionary<string, object> nested)
            status = GetString(nested, "status");
        status = status?.ToUpperInvariant();
        return status is "PAID" or "SUCCESS" or "COMPLETED" or "SUCCEEDED";
    }

    private static Dictionary<string, object> JsonElementToDictionary(JsonElement element)
    {
        var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        foreach (var prop in element.EnumerateObject())
        {
            dict[prop.Name] = JsonElementToObject(prop.Value);
        }
        return dict;
    }

    private static object JsonElementToObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => JsonElementToDictionary(element),
            JsonValueKind.Array => element.EnumerateArray().Select(JsonElementToObject).ToList(),
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.TryGetInt32(out var i) ? i : element.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => (object?)null!,
            _ => element.GetRawText()
        };
    }
}
