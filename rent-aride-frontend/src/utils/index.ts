const CURRENCY = 'IQD' as const

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: CURRENCY,
    maximumFractionDigits: 0,
    minimumFractionDigits: 0,
  }).format(amount)
}

export function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString()
}

export { parseUserFromToken } from './jwt'
export { useFormErrors } from './useFormErrors'
export type { ApiErrorLike } from './useFormErrors'
export { usePagination } from './usePagination'
