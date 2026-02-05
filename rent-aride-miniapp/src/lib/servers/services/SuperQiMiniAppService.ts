import { httpClient } from '../core/HttpClient.js'
import { API_ENDPOINTS } from '../config/api.js'
import type { ApiResponse } from '../../types/api.js'
import type {
  SuperQiPaymentResponse,
  SuperQiRefundRequest,
  SuperQiRefundResponse,
  SuperQiNotificationRequest,
  SuperQiNotificationResponse,
  SuperQiPrepareAgreementRequest,
  SuperQiPrepareAuthResponse,
  SuperQiApplyTokenRequest,
  SuperQiApplyTokenResponse,
  SuperQiAgreementPayRequest,
} from '../../types/superqi.js'

/**
 * Service for SuperQi MiniApp API (api/superqi).
 * Mirrors RentARide.Api SuperQiMiniAppController endpoints.
 */
export class SuperQiMiniAppService {
  private base = API_ENDPOINTS.superqi

  /**
   * Creates a SuperQi payment for an existing invoice. Returns payment URL for redirect.
   * [Authorize] POST api/superqi/payment/invoice/{invoiceId}
   */
  async createPaymentForInvoice(
    invoiceId: string,
    finishUrl: string
  ): Promise<ApiResponse<SuperQiPaymentResponse>> {
    const path = `${this.base}/payment/invoice/${invoiceId}?finishUrl=${encodeURIComponent(finishUrl)}`
    return httpClient.post<SuperQiPaymentResponse>(path)
  }

  /** Create test payment (1 IQD). [Authorize] POST api/superqi/payment/create */
  async createPayment(): Promise<ApiResponse<SuperQiPaymentResponse>> {
    return httpClient.post<SuperQiPaymentResponse>(`${this.base}/payment/create`)
  }

  /** Refund a payment. [Authorize] POST api/superqi/payment/refund */
  async refundPayment(
    request: SuperQiRefundRequest
  ): Promise<ApiResponse<SuperQiRefundResponse>> {
    return httpClient.post<SuperQiRefundResponse>(`${this.base}/payment/refund`, request)
  }

  /** Send inbox notification. [Authorize] POST api/superqi/notification/inbox */
  async sendInbox(
    request: SuperQiNotificationRequest
  ): Promise<ApiResponse<SuperQiNotificationResponse>> {
    return httpClient.post<SuperQiNotificationResponse>(
      `${this.base}/notification/inbox`,
      request
    )
  }

  /** Send push notification. [Authorize] POST api/superqi/notification/push */
  async sendPush(
    request: SuperQiNotificationRequest
  ): Promise<ApiResponse<SuperQiNotificationResponse>> {
    return httpClient.post<SuperQiNotificationResponse>(
      `${this.base}/notification/push`,
      request
    )
  }

  /** Prepare agreement (recurring). POST api/superqi/agreement/prepare */
  async prepareAgreement(
    request: SuperQiPrepareAgreementRequest
  ): Promise<ApiResponse<SuperQiPrepareAuthResponse>> {
    return httpClient.post<SuperQiPrepareAuthResponse>(
      `${this.base}/agreement/prepare`,
      request
    )
  }

  /** Apply agreement token from auth code. POST api/superqi/agreement/apply-token */
  async applyAgreementToken(
    request: SuperQiApplyTokenRequest
  ): Promise<ApiResponse<SuperQiApplyTokenResponse>> {
    return httpClient.post<SuperQiApplyTokenResponse>(
      `${this.base}/agreement/apply-token`,
      request
    )
  }

  /** Execute agreement payment. POST api/superqi/agreement/pay */
  async executeAgreementPayment(
    request: SuperQiAgreementPayRequest
  ): Promise<ApiResponse<SuperQiPaymentResponse>> {
    return httpClient.post<SuperQiPaymentResponse>(
      `${this.base}/agreement/pay`,
      request
    )
  }
}

export const superQiMiniAppService = new SuperQiMiniAppService()
