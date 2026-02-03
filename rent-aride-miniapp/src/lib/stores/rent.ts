import { writable, get } from 'svelte/store'
import * as rentalsApi from '../api/rentals.js'
import type { RentalHistoryItemDto } from '../types/rental.js'

export const items = writable<RentalHistoryItemDto[]>([])
export const totalPages = writable(0)
export const totalCount = writable(0)
export const pageIndex = writable(1)
export const pageSize = writable(10)
export const statusFilter = writable<string | undefined>(undefined)
export const loading = writable(false)

export async function load(): Promise<void> {
  loading.set(true)
  try {
    const res = await rentalsApi.getMyHistory({
      pageNumber: get(pageIndex),
      pageSize: get(pageSize),
      status: get(statusFilter) ?? undefined,
    })
    items.set(res.items)
    totalPages.set(res.totalPages)
    totalCount.set(res.totalCount)
  } catch {
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
