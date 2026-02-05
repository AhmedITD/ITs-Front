using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Api.Common;
using RentARide.Application.DTOs.requests.SuperQi;
using RentARide.Application.DTOs.responses.Common;
using RentARide.Application.DTOs.responses.SuperQi;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Api.Controllers;

/// <summary>
/// SuperQi MiniApp endpoints for payment, refund, notification, and agreement flows.
/// Port of backend-node/src/api endpoints.
/// </summary>
[Route("api/superqi")]
public class SuperQiMiniAppController(
    ISuperQiMiniAppService miniAppService,
    ICurrentUser currentUser) : BaseController
{
    /// <summary>Creates an online purchase payment for an existing invoice. Returns payment URL for redirect.</summary>
    [Authorize]
    [HttpPost("payment/invoice/{invoiceId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPaymentResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPaymentResponse>>> CreatePaymentForInvoice(Guid invoiceId, [FromQuery] string finishUrl, CancellationToken ct)
    {
        var result = await miniAppService.CreatePaymentForInvoiceAsync(invoiceId, finishUrl, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // Create test payment 1 IQD
    [Authorize]
    [HttpPost("payment/create")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPaymentResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPaymentResponse>>> CreatePayment(CancellationToken ct)
    {
        var result = await miniAppService.CreatePaymentAsync(currentUser.Id.ToString(), ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // Refund a payment via Alipay+
    [Authorize]
    [HttpPost("payment/refund")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiRefundResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiRefundResponse>>> RefundPayment(
        [FromBody] SuperQiRefundRequest request, CancellationToken ct)
    {
        var result = await miniAppService.RefundPaymentAsync(request.PaymentId, request.Amount, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ========== Notification Endpoints ==========

    [Authorize]
    [HttpPost("notification/inbox")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiNotificationResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiNotificationResponse>>> SendInbox(
        [FromBody] SuperQiNotificationRequest request, CancellationToken ct)
    {
        var result = await miniAppService.SendInboxAsync(request.AccessToken, request.Title, request.Content, request.Url, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPost("notification/push")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiNotificationResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiNotificationResponse>>> SendPush(
        [FromBody] SuperQiNotificationRequest request, CancellationToken ct)
    {
        var result = await miniAppService.SendPushAsync(request.AccessToken, request.Title, request.Content, request.Url, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ========== Agreement (Recurring Payment) Endpoints ==========

    // Prepare agreement contract for recurring payments
    [HttpPost("agreement/prepare")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPrepareAuthResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPrepareAuthResponse>>> PrepareAgreement(
        [FromBody] SuperQiPrepareAgreementRequest request, CancellationToken ct)
    {
        var result = await miniAppService.PrepareAgreementAsync(request.ContractDescription, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("agreement/apply-token")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiApplyTokenResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiApplyTokenResponse>>> ApplyAgreementToken(
        [FromBody] SuperQiApplyTokenRequest request, CancellationToken ct)
    {
        var result = await miniAppService.ApplyAgreementTokenAsync(request.AuthCode, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // Execute agreement payment
    [HttpPost("agreement/pay")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPaymentResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPaymentResponse>>> ExecuteAgreementPayment(
        [FromBody] SuperQiAgreementPayRequest request, CancellationToken ct)
    {
        var result = await miniAppService.ExecuteAgreementPaymentAsync(request, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
