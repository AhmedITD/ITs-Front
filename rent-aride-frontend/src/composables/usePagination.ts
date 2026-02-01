import { ref, computed } from 'vue'

export function usePagination(initialPage = 1, pageSize = 10) {
  const pageIndex = ref(initialPage)
  const totalPages = ref(0)

  const hasPreviousPage = computed(() => pageIndex.value > 1)
  const hasNextPage = computed(() => pageIndex.value < totalPages.value)

  function goNext() {
    if (pageIndex.value < totalPages.value) pageIndex.value++
  }

  function goPrev() {
    if (pageIndex.value > 1) pageIndex.value--
  }

  function setTotalPages(value: number) {
    totalPages.value = value
  }

  return {
    pageIndex,
    totalPages,
    hasPreviousPage,
    hasNextPage,
    goNext,
    goPrev,
    setTotalPages,
  }
}
