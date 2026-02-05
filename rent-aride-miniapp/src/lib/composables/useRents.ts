/**
 * Rents composable (same idea as frontend useRents).
 * Exports store state + initRents.
 */
import {
  items,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  statusFilter,
  startDateFilter,
  endDateFilter,
  minPriceFilter,
  maxPriceFilter,
  searchQuery,
  loading,
  showingText,
  filterErrors,
  load,
  setPageSize,
  resetToFirstPage,
} from '../stores/rent.js'

export {
  items,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  statusFilter,
  startDateFilter,
  endDateFilter,
  minPriceFilter,
  maxPriceFilter,
  searchQuery,
  loading,
  showingText,
  filterErrors,
  load,
  setPageSize,
  resetToFirstPage,
}

export function initRents(initialPageSize = 10): void {
  setPageSize(initialPageSize)
}
