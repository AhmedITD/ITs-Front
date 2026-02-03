<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import { storeToRefs } from 'pinia'
import { rentStore, initRents } from '@/composables/Rent/useRents'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'
import Pagination from '@/components/shared/Pagination.vue'
import SercchBar from '@/components/shared/SercchBar.vue'
import StatusFilter from '@/components/shared/Filters/Rent/StatusFilter.vue'
import RentalHistoryTable from '@/components/shared/Tables/RentalHistoryTable.vue'

const PAGE_SIZE_OPTIONS = [5, 10, 25, 50] as const

const {
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
} = storeToRefs(rentStore)

let stopWatcher: (() => void) | undefined
onMounted(() => {
  stopWatcher = initRents(10)
})
onUnmounted(() => {
  stopWatcher?.()
})
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">My rentals</h1>
    <div
      v-if="filterErrors.general"
      class="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
      role="alert"
    >
      {{ filterErrors.general }}
    </div>
    <div class="flex flex-wrap items-end gap-4">
      <div class="min-w-[200px] max-w-xs">
        <label class="mb-1 block text-sm font-medium text-gray-700">Search</label>
        <SercchBar v-model="searchQuery" placeholder="Vehicle or plate…" />
      </div>
      <StatusFilter
        v-model="statusFilter"
        label="Status"
        placeholder="Select status"
      />
      <div class="min-w-[140px]">
        <label for="start-date" class="mb-1 block text-sm font-medium text-gray-700">From date</label>
        <input
          id="start-date"
          v-model="startDateFilter"
          type="date"
          :class="[
            'rounded-lg border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-1',
            filterErrors.startDateFrom
              ? 'border-red-500 focus:border-red-500 focus:ring-red-500'
              : 'border-gray-300 focus:border-indigo-500 focus:ring-indigo-500'
          ]"
        >
        <p v-if="filterErrors.startDateFrom" class="mt-1 text-sm text-red-600">
          {{ filterErrors.startDateFrom }}
        </p>
      </div>
      <div class="min-w-[140px]">
        <label for="end-date" class="mb-1 block text-sm font-medium text-gray-700">To date</label>
        <input
          id="end-date"
          v-model="endDateFilter"
          type="date"
          :class="[
            'rounded-lg border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-1',
            filterErrors.startDateTo
              ? 'border-red-500 focus:border-red-500 focus:ring-red-500'
              : 'border-gray-300 focus:border-indigo-500 focus:ring-indigo-500'
          ]"
        >
        <p v-if="filterErrors.startDateTo" class="mt-1 text-sm text-red-600">
          {{ filterErrors.startDateTo }}
        </p>
      </div>
      <div>
        <label for="min-price" class="mb-1 block text-sm font-medium text-gray-700">Min price</label>
        <input
          id="min-price"
          v-model.number="minPriceFilter"
          type="number"
          min="0"
          step="0.01"
          placeholder="0"
          :class="[
            'w-28 rounded-lg border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-1',
            filterErrors.minPrice
              ? 'border-red-500 focus:border-red-500 focus:ring-red-500'
              : 'border-gray-300 focus:border-indigo-500 focus:ring-indigo-500'
          ]"
        >
        <p v-if="filterErrors.minPrice" class="mt-1 text-sm text-red-600">
          {{ filterErrors.minPrice }}
        </p>
      </div>
      <div>
        <label for="max-price" class="mb-1 block text-sm font-medium text-gray-700">Max price</label>
        <input
          id="max-price"
          v-model.number="maxPriceFilter"
          type="number"
          min="0"
          step="0.01"
          placeholder="Any"
          :class="[
            'w-28 rounded-lg border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-1',
            filterErrors.maxPrice
              ? 'border-red-500 focus:border-red-500 focus:ring-red-500'
              : 'border-gray-300 focus:border-indigo-500 focus:ring-indigo-500'
          ]"
        >
        <p v-if="filterErrors.maxPrice" class="mt-1 text-sm text-red-600">
          {{ filterErrors.maxPrice }}
        </p>
      </div>
    </div>
    <div v-if="loading" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <div v-else-if="items.length === 0" class="py-8 text-center text-gray-500">
      No rentals yet. <router-link to="/rentals/new" class="text-indigo-600 hover:underline">Book a car</router-link>.
    </div>
    <div v-else>
      <RentalHistoryTable :items="items" />
      <Pagination
        v-if="totalPages > 1"
        v-model:current-page="pageIndex"
        :total-pages="totalPages"
        :page-size="pageSize"
        :total-count="totalCount"
      />
    </div>
  </div>
</template>
