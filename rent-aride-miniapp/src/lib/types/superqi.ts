/** Request/response types for SuperQi MiniApp API (api/superqi). */

export interface SuperQiPaymentResponse {
  success: boolean
  paymentUrl?: string
  paymentId?: string
  amount: number
  error?: string
}

export interface SuperQiRefundRequest {
  paymentId: string
  amount: number
}

export interface SuperQiRefundResponse {
  success: boolean
  status: string
  refundId?: string
  refundTime?: string
  message?: string
  resultStatus?: string
  resultCode?: string
  resultMessage?: string
}

export interface SuperQiNotificationRequest {
  accessToken: string
  title: string
  content: string
  url?: string
}

export interface SuperQiNotificationResponse {
  success: boolean
  status: string
  messageId?: string
  message?: string
  resultStatus?: string
  resultCode?: string
  resultMessage?: string
}

export interface SuperQiPrepareAgreementRequest {
  contractDescription: string
}

export interface SuperQiPrepareAuthResponse {
  success: boolean
  authUrl?: string
  resultStatus?: string
  resultCode?: string
  resultMessage?: string
}

export interface SuperQiApplyTokenRequest {
  authCode: string
}

export interface SuperQiApplyTokenResponse {
  success: boolean
  accessToken?: string
  customerId?: string
  accessTokenExpiryTime?: string
  resultStatus?: string
  resultCode?: string
  resultMessage?: string
}

export interface SuperQiAgreementPayRequest {
  accessToken: string
  customerId: string
  amount: number
  orderDescription?: string
  invoiceId?: string
}
