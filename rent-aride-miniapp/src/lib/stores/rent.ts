import { writable, get, derived } from 'svelte/store'
import * as rentalsApi from '../api/rentals.js'
import type { RentalHistoryItemDto } from '../types/rental.js'

export type FilterErrors = {
  general?: string
  startDateFrom?: string
  startDateTo?: string
  minPrice?: string
  maxPrice?: string
}

function getFieldError(errors: Record<string, string[]> | undefined, key: string): string {
  if (!errors || !Object.keys(errors).length) return ''
  const pascal = key.charAt(0).toUpperCase() + key.slice(1)
  const arr = errors[key] ?? errors[pascal]
  return arr?.length ? (arr[0] ?? '') : ''
}

export const items = writable<RentalHistoryItemDto[]>([])
export const totalPages = writable(0)
export const totalCount = writable(0)
export const pageIndex = writable(1)
export const pageSize = writable(10)
export const statusFilter = writable<string | undefined>(undefined)
export const startDateFilter = writable('')
export const endDateFilter = writable('')
export const minPriceFilter = writable<number | undefined>(undefined)
export const maxPriceFilter = writable<number | undefined>(undefined)
export const searchQuery = writable('')
export const loading = writable(false)
export const filterErrors = writable<FilterErrors>({})

export const rangeStart = derived([pageIndex, pageSize], ([$pageIndex, $pageSize]) =>
  ($pageIndex - 1) * $pageSize + 1
)
export const rangeEnd = derived(
  [pageIndex, pageSize, totalCount],
  ([$pageIndex, $pageSize, $totalCount]) =>
    Math.min($pageIndex * $pageSize, $totalCount)
)
export const showingText = derived(
  [rangeStart, rangeEnd, totalCount],
  ([$rangeStart, $rangeEnd, $totalCount]) => {
    if ($totalCount === 0) return ''
    return `Showing ${$rangeStart}–${$rangeEnd} of ${$totalCount} rentals`
  }
)

export function validateFilters(): boolean {
  const startFrom = get(startDateFilter) ? new Date(get(startDateFilter)) : null
  const endTo = get(endDateFilter) ? new Date(get(endDateFilter)) : null
  const errors: FilterErrors = {}
  if (startFrom && endTo && startFrom > endTo) {
    errors.startDateFrom = 'From date must be before or equal to to date.'
    errors.startDateTo = 'From date must be before or equal to to date.'
    filterErrors.set(errors)
    return false
  }
  const min = get(minPriceFilter)
  const max = get(maxPriceFilter)
  if (min != null && min < 0) {
    errors.minPrice = 'Min price must be non-negative.'
    filterErrors.set(errors)
    return false
  }
  if (max != null && max < 0) {
    errors.maxPrice = 'Max price must be non-negative.'
    filterErrors.set(errors)
    return false
  }
  if (min != null && max != null && min > max) {
    errors.minPrice = 'Min price must be less than or equal to max price.'
    errors.maxPrice = 'Min price must be less than or equal to max price.'
    filterErrors.set(errors)
    return false
  }
  filterErrors.set({})
  return true
}

export async function load(): Promise<void> {
  filterErrors.set({})
  if (!validateFilters()) return
  loading.set(true)
  try {
    const res = await rentalsApi.getMyHistory({
      pageNumber: get(pageIndex),
      pageSize: get(pageSize),
      status: get(statusFilter) ?? undefined,
      startDateFrom: get(startDateFilter) || undefined,
      startDateTo: get(endDateFilter) || undefined,
      minPrice: get(minPriceFilter),
      maxPrice: get(maxPriceFilter),
      searchTerm: get(searchQuery)?.trim() || undefined,
    })
    items.set(res.items)
    totalPages.set(res.totalPages)
    totalCount.set(res.totalCount)
  } catch (err: unknown) {
    const e = err as { message?: string; errors?: Record<string, string[]> }
    const errors = e?.errors ?? {}
    filterErrors.set({
      general: e?.message,
      startDateFrom: getFieldError(errors, 'StartDateFrom'),
      startDateTo: getFieldError(errors, 'StartDateTo'),
      minPrice: getFieldError(errors, 'MinPrice'),
      maxPrice: getFieldError(errors, 'MaxPrice'),
    })
    items.set([])
  } finally {
    loading.set(false)
  }
}

export function setPageSize(size: number): void {
  pageSize.set(size)
}

export function resetToFirstPage(): void {
  pageIndex.set(1)
}
