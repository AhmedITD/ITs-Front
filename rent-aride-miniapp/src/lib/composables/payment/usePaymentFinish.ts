/**
 * Payment finish composable: same concept as frontend usePaymentFinish.
 * Parses query from hash (#/payment/finish?requestId=...).
 */

export function getPaymentFinishQuery(): {
  requestId: string
  paymentId: string
  paymentType: string
  status: string
} {
  if (typeof window === 'undefined') {
    return { requestId: '', paymentId: '', paymentType: '', status: '' }
  }
  const hash = window.location.hash
  const q = hash.includes('?') ? hash.slice(hash.indexOf('?') + 1) : ''
  const params = new URLSearchParams(q)
  const status = (params.get('status') ?? '').toUpperCase()
  return {
    requestId: params.get('requestId') ?? '',
    paymentId: params.get('paymentId') ?? '',
    paymentType: params.get('paymentType') ?? '',
    status,
  }
}

export function getStatusLabel(statusVal: string | undefined): string {
  switch (statusVal) {
    case 'SUCCESS':
      return 'Payment successful'
    case 'FAILED':
    case 'CANCELLED':
      return (statusVal.charAt(0) + statusVal.slice(1).toLowerCase()) as string
    default:
      return statusVal || 'Unknown'
  }
}

export function isSuccessStatus(statusVal: string): boolean {
  return statusVal === 'SUCCESS'
}

export function isFailedStatus(statusVal: string): boolean {
  return !!statusVal && statusVal !== 'SUCCESS'
}
