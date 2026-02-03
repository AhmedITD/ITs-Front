/**
 * Rents composable: init + store exports (same idea as frontend useRents).
 */
import {
  items,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  statusFilter,
  loading,
  load,
  setPageSize,
  resetToFirstPage,
} from './stores/rent.js'

export {
  items,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  statusFilter,
  loading,
  load,
  setPageSize,
  resetToFirstPage,
}

export function initRents(initialPageSize = 10): void {
  setPageSize(initialPageSize)
}
