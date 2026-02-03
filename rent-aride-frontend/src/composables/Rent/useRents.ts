import { watch } from 'vue'
import { useRentStore } from '@/stores/rent'

const rentStore = useRentStore()

function initRents(initialPageSize = 10) {
  rentStore.setPageSize(initialPageSize)
  const stopFilters = watch(
    () => [
      rentStore.statusFilter,
      rentStore.startDateFilter,
      rentStore.endDateFilter,
      rentStore.minPriceFilter,
      rentStore.maxPriceFilter,
      rentStore.searchQuery,
    ],
    () => rentStore.resetToFirstPage()
  )
  const stopLoad = watch(
    () => [
      rentStore.pageIndex,
      rentStore.pageSize,
      rentStore.statusFilter,
      rentStore.startDateFilter,
      rentStore.endDateFilter,
      rentStore.minPriceFilter,
      rentStore.maxPriceFilter,
      rentStore.searchQuery,
    ],
    () => rentStore.load(),
    { immediate: true }
  )
  return () => {
    stopFilters()
    stopLoad()
  }
}

// functions
export { initRents }
// stores
export { rentStore }
