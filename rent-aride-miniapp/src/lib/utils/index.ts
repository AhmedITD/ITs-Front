export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'IQD',
    maximumFractionDigits: 0,
    minimumFractionDigits: 0,
  }).format(amount ?? 0)
}

export function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString()
}

export { useFormErrors } from './useFormErrors.js'
export type { ApiErrorLike } from './useFormErrors.js'
