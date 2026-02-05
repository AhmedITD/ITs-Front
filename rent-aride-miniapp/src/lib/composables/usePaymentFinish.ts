/**
 * Composable for payment finish page (same concept as frontend usePaymentFinish).
 * Must be called from component so query is read from current location.
 */
import { push } from 'svelte-spa-router'

function getPaymentFinishQuery(): {
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

function getStatusLabel(statusVal: string | undefined): string {
  switch (statusVal) {
    case 'SUCCESS':
      return 'Payment successful'
    case 'FAILED':
    case 'CANCELLED':
      return statusVal.charAt(0) + statusVal.slice(1).toLowerCase()
    default:
      return statusVal || 'Unknown'
  }
}

function isSuccessStatus(statusVal: string): boolean {
  return statusVal === 'SUCCESS'
}

function isFailedStatus(statusVal: string): boolean {
  return !!statusVal && statusVal !== 'SUCCESS'
}

function goToRentals(): void {
  push('/rentals')
}

function goHome(): void {
  push('/')
}

export default function usePaymentFinish() {
  const q = getPaymentFinishQuery()
  return {
    requestId: q.requestId,
    paymentId: q.paymentId,
    paymentType: q.paymentType,
    status: q.status,
    statusLabel: getStatusLabel(q.status),
    isSuccess: isSuccessStatus(q.status),
    isFailed: isFailedStatus(q.status),
    getStatusLabel,
    isSuccessStatus,
    isFailedStatus,
    goToRentals,
    goHome,
  }
}
