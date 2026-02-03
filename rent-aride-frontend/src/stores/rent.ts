import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { rentalService } from '@/servers/services/RentalService'
import type { RentalHistoryItemDto } from '@/types/rental'
import type { ApiException } from '@/types/api'

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
  return arr?.length ? arr[0] ?? '' : ''
}

export const useRentStore = defineStore('rent', () => {
  const items = ref<RentalHistoryItemDto[]>([])
  const totalPages = ref(0)
  const totalCount = ref(0)
  const pageIndex = ref(1)
  const pageSize = ref(10)
  const statusFilter = ref<string | undefined>(undefined)
  const startDateFilter = ref<string>('')
  const endDateFilter = ref<string>('')
  const minPriceFilter = ref<number | undefined>(undefined)
  const maxPriceFilter = ref<number | undefined>(undefined)
  const searchQuery = ref<string>('')
  const loading = ref(false)
  const filterErrors = ref<FilterErrors>({})

  const rangeStart = computed(() => (pageIndex.value - 1) * pageSize.value + 1)
  const rangeEnd = computed(() =>
    Math.min(pageIndex.value * pageSize.value, totalCount.value)
  )
  const showingText = computed(() => {
    if (totalCount.value === 0) return ''
    return `Showing ${rangeStart.value}–${rangeEnd.value} of ${totalCount.value} rentals`
  })

  function validateFilters(): boolean {
    filterErrors.value = {}
    const startFrom = startDateFilter.value ? new Date(startDateFilter.value) : null
    const endTo = endDateFilter.value ? new Date(endDateFilter.value) : null
    if (startFrom && endTo && startFrom > endTo) {
      filterErrors.value.startDateFrom = 'From date must be before or equal to to date.'
      filterErrors.value.startDateTo = 'From date must be before or equal to to date.'
      return false
    }
    const min = minPriceFilter.value
    const max = maxPriceFilter.value
    if (min != null && min < 0) {
      filterErrors.value.minPrice = 'Min price must be non-negative.'
      return false
    }
    if (max != null && max < 0) {
      filterErrors.value.maxPrice = 'Max price must be non-negative.'
      return false
    }
    if (min != null && max != null && min > max) {
      filterErrors.value.minPrice = 'Min price must be less than or equal to max price.'
      filterErrors.value.maxPrice = 'Min price must be less than or equal to max price.'
      return false
    }
    return true
  }

  async function load() {
    filterErrors.value = {}
    if (!validateFilters()) {
      return
    }
    loading.value = true
    try {
      const res = await rentalService.getMyHistory({
        pageNumber: pageIndex.value,
        pageSize: pageSize.value,
        status: statusFilter.value ?? undefined,
        startDateFrom: startDateFilter.value || undefined,
        startDateTo: endDateFilter.value || undefined,
        minPrice: minPriceFilter.value,
        maxPrice: maxPriceFilter.value,
        searchTerm: searchQuery.value?.trim() || undefined,
      })
      if (res.data) {
        items.value = res.data.items ?? []
        totalPages.value = res.data.totalPages ?? 0
        totalCount.value = res.data.totalCount ?? 0
      }
    } catch (err) {
      const e = err as ApiException
      const errors = e.errors ?? {}
      filterErrors.value = {
        general: e.message,
        startDateFrom: getFieldError(errors, 'StartDateFrom'),
        startDateTo: getFieldError(errors, 'StartDateTo'),
        minPrice: getFieldError(errors, 'MinPrice'),
        maxPrice: getFieldError(errors, 'MaxPrice'),
      }
      const emptyKeyMsg = errors['']?.[0]
      if (emptyKeyMsg) {
        filterErrors.value.general = filterErrors.value.general
          ? `${filterErrors.value.general} ${emptyKeyMsg}`
          : emptyKeyMsg
      }
    } finally {
      loading.value = false
    }
  }

  function setPageSize(size: number) {
    pageSize.value = size
  }

  function resetToFirstPage() {
    pageIndex.value = 1
  }

  return {
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
    rangeStart,
    rangeEnd,
    showingText,
    load,
    setPageSize,
    resetToFirstPage,
    validateFilters,
    filterErrors,
  }
})
