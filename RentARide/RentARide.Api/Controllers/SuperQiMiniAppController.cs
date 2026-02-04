using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Api.Common;
using RentARide.Application.DTOs.Requests.SuperQi;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.Responses.SuperQi;
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
    // ========== Payment Endpoints ==========

    /// <summary>Create test payment via Alipay+ (demo: 1 IQD).</summary>
    [Authorize]
    [HttpPost("payment/create")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPaymentResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPaymentResponse>>> CreatePayment(CancellationToken ct)
    {
        var result = await miniAppService.CreatePaymentAsync(currentUser.Id.ToString(), ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Refund a payment via Alipay+.</summary>
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

    /// <summary>Send inbox notification to user via Alipay+.</summary>
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

    /// <summary>Send push notification to user via Alipay+.</summary>
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

    /// <summary>Prepare agreement contract for recurring payments.</summary>
    [HttpPost("agreement/prepare")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPrepareAuthResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPrepareAuthResponse>>> PrepareAgreement(
        [FromBody] SuperQiPrepareAgreementRequest request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.ContractDescription))
            return BadRequest(ApiResponse<SuperQiPrepareAuthResponse>.ErrorResponse("contractDescription is required"));

        var result = await miniAppService.PrepareAgreementAsync(request.ContractDescription, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Exchange auth code for access token (agreement flow).</summary>
    [HttpPost("agreement/apply-token")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiApplyTokenResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiApplyTokenResponse>>> ApplyAgreementToken(
        [FromBody] SuperQiApplyTokenRequest request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.AuthCode))
            return BadRequest(ApiResponse<SuperQiApplyTokenResponse>.ErrorResponse("authCode is required"));

        var result = await miniAppService.ApplyAgreementTokenAsync(request.AuthCode, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Execute agreement payment (deduct from wallet automatically).</summary>
    [HttpPost("agreement/pay")]
    [ProducesResponseType(typeof(ApiResponse<SuperQiPaymentResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ApiResponse<SuperQiPaymentResponse>>> ExecuteAgreementPayment(
        [FromBody] SuperQiAgreementPayRequest request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.AccessToken))
            return BadRequest(ApiResponse<SuperQiPaymentResponse>.ErrorResponse("accessToken is required"));

        if (string.IsNullOrEmpty(request.CustomerId))
            return BadRequest(ApiResponse<SuperQiPaymentResponse>.ErrorResponse("customerId is required"));

        if (request.Amount <= 0)
            return BadRequest(ApiResponse<SuperQiPaymentResponse>.ErrorResponse("amount must be greater than 0"));

        var result = await miniAppService.ExecuteAgreementPaymentAsync(
            request.AccessToken,
            request.CustomerId,
            request.Amount,
            request.Currency ?? "IQD",
            request.OrderDescription ?? "Agreement payment",
            ct);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
