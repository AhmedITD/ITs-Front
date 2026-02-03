<script setup lang="ts">
import { computed } from 'vue'

const props = withDefaults(
  defineProps<{
    currentPage: number
    totalPages: number
    pageSize: number
    totalCount?: number
  }>(),
  { totalCount: undefined }
)

const emit = defineEmits<{
  'update:currentPage': [page: number]
}>()

const startItem = computed(() => (props.currentPage - 1) * props.pageSize + 1)
const endItem = computed(() =>
  props.totalCount != null
    ? Math.min(props.currentPage * props.pageSize, props.totalCount)
    : props.currentPage * props.pageSize
)

const showResultsText = computed(() => props.totalCount != null && props.totalCount > 0)

const pageNumbers = computed(() => {
  const current = props.currentPage
  const total = props.totalPages
  if (total <= 1) return []
  const pages: (number | 'ellipsis')[] = [1]
  if (current - 2 > 1) pages.push('ellipsis')
  for (const p of [current - 1, current, current + 1]) {
    if (p > 1 && p < total && !pages.includes(p)) pages.push(p)
  }
  if (current + 2 < total) pages.push('ellipsis')
  if (total > 1 && !pages.includes(total)) pages.push(total)
  return pages
})

function goTo(page: number) {
  if (page >= 1 && page <= props.totalPages) emit('update:currentPage', page)
}

function prev() {
  if (props.currentPage > 1) emit('update:currentPage', props.currentPage - 1)
}

function next() {
  if (props.currentPage < props.totalPages) emit('update:currentPage', props.currentPage + 1)
}

const canPrev = computed(() => props.currentPage > 1)
const canNext = computed(() => props.currentPage < props.totalPages)
</script>

<template>
  <div class="flex items-center justify-between border-t border-gray-200 px-4 py-3 sm:px-6">
    <div class="flex flex-1 justify-between sm:hidden">
      <button
        type="button"
        class="relative inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none"
        :disabled="!canPrev"
        @click="prev"
      >
        Previous
      </button>
      <button
        type="button"
        class="relative ml-3 inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none"
        :disabled="!canNext"
        @click="next"
      >
        Next
      </button>
    </div>
    <div class="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
      <div v-if="showResultsText">
        <p class="text-sm text-gray-600">
          Showing
          <span class="font-medium text-gray-900">{{ startItem }}</span>
          to
          <span class="font-medium text-gray-900">{{ endItem }}</span>
          of
          <span class="font-medium text-gray-900">{{ totalCount }}</span>
          results
        </p>
      </div>
      <div class="sm:ml-auto">
        <nav aria-label="Pagination" class="isolate inline-flex -space-x-px rounded-md shadow-sm">
          <button
            type="button"
            class="relative inline-flex items-center rounded-l-md border border-gray-300 bg-white px-2 py-2 text-gray-500 hover:bg-gray-50 focus:z-20 focus:outline-offset-0 disabled:opacity-50 disabled:pointer-events-none"
            :disabled="!canPrev"
            :aria-disabled="!canPrev"
            @click="prev"
          >
            <span class="sr-only">Previous</span>
            <svg viewBox="0 0 20 20" fill="currentColor" aria-hidden="true" class="size-5">
              <path
                d="M11.78 5.22a.75.75 0 0 1 0 1.06L8.06 10l3.72 3.72a.75.75 0 1 1-1.06 1.06l-4.25-4.25a.75.75 0 0 1 0-1.06l4.25-4.25a.75.75 0 0 1 1.06 0Z"
                clip-rule="evenodd"
                fill-rule="evenodd"
              />
            </svg>
          </button>
          <template v-for="(item, i) in pageNumbers" :key="i">
            <span
              v-if="item === 'ellipsis'"
              class="relative inline-flex items-center border border-gray-300 border-l-0 bg-white px-4 py-2 text-sm font-semibold text-gray-500"
            >
              ...
            </span>
            <button
              v-else
              type="button"
              :aria-current="item === currentPage ? 'page' : undefined"
              class="relative inline-flex items-center border border-gray-300 border-l-0 px-4 py-2 text-sm font-semibold focus:z-20 focus:outline-offset-0"
              :class="
                item === currentPage
                  ? 'z-10 border border-l-0 border-indigo-600 bg-indigo-600 text-white focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600'
                  : 'border border-gray-300 border-l-0 bg-white text-gray-700 hover:bg-gray-50 focus:outline-offset-0'
              "
              @click="goTo(item)"
            >
              {{ item }}
            </button>
          </template>
          <button
            type="button"
            class="relative inline-flex items-center rounded-r-md border border-gray-300 border-l-0 bg-white px-2 py-2 text-gray-500 hover:bg-gray-50 focus:z-20 focus:outline-offset-0 disabled:opacity-50 disabled:pointer-events-none"
            :disabled="!canNext"
            :aria-disabled="!canNext"
            @click="next"
          >
            <span class="sr-only">Next</span>
            <svg viewBox="0 0 20 20" fill="currentColor" aria-hidden="true" class="size-5">
              <path
                d="M8.22 5.22a.75.75 0 0 1 1.06 0l4.25 4.25a.75.75 0 0 1 0 1.06l-4.25 4.25a.75.75 0 0 1-1.06-1.06L11.94 10 8.22 6.28a.75.75 0 0 1 0-1.06Z"
                clip-rule="evenodd"
                fill-rule="evenodd"
              />
            </svg>
          </button>
        </nav>
      </div>
    </div>
  </div>
</template>
