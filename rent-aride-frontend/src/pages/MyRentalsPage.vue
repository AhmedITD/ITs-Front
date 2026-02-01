<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { rentalService } from '@/servers/services/RentalService'
import { formatCurrency } from '@/utils'
import type { RentalHistoryItemDto } from '@/types/rental'

const items = ref<RentalHistoryItemDto[]>([])
const totalPages = ref(0)
const totalCount = ref(0)
const pageIndex = ref(1)
const pageSize = ref(10)
const loading = ref(false)

const PAGE_SIZE_OPTIONS = [5, 10, 25, 50] as const

const rangeStart = computed(() => (pageIndex.value - 1) * pageSize.value + 1)
const rangeEnd = computed(() =>
  Math.min(pageIndex.value * pageSize.value, totalCount.value)
)
const showingText = computed(() => {
  if (totalCount.value === 0) return ''
  return `Showing ${rangeStart.value}–${rangeEnd.value} of ${totalCount.value} rentals`
})

function pageNumbers(): number[] {
  const total = totalPages.value
  if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1)
  const pages: number[] = []
  if (pageIndex.value <= 4) {
    pages.push(1, 2, 3, 4, 5, -1, total)
  } else if (pageIndex.value >= total - 3) {
    pages.push(1, -1, total - 4, total - 3, total - 2, total - 1, total)
  } else {
    pages.push(1, -1, pageIndex.value - 1, pageIndex.value, pageIndex.value + 1, -1, total)
  }
  return pages
}

async function load() {
  loading.value = true
  try {
    const res = await rentalService.getMyHistory(pageIndex.value, pageSize.value)
    if (res.data) {
      items.value = res.data.items ?? []
      totalPages.value = res.data.totalPages ?? 0
      totalCount.value = res.data.totalCount ?? 0
    }
  } finally {
    loading.value = false
  }
}

function formatDate(s: string) {
  return new Date(s).toLocaleDateString()
}

function goPage(p: number) {
  if (p < 1 || p > totalPages.value) return
  pageIndex.value = p
  load()
}

function onPageSizeChange() {
  pageIndex.value = 1
  load()
}

onMounted(load)
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">My rentals</h1>
    <div v-if="loading" class="py-8 text-center text-gray-500">Loading…</div>
    <div v-else-if="items.length === 0" class="py-8 text-center text-gray-500">
      No rentals yet. <router-link to="/rentals/new" class="text-indigo-600 hover:underline">Book a car</router-link>.
    </div>
    <div v-else>
      <div class="flex flex-wrap items-center justify-between gap-4">
        <p class="text-sm text-gray-600">{{ showingText }}</p>
        <div class="flex items-center gap-2">
          <label for="page-size" class="text-sm text-gray-600">Per page:</label>
          <select
            id="page-size"
            v-model.number="pageSize"
            class="rounded-md border border-gray-300 px-2 py-1 text-sm"
            @change="onPageSizeChange"
          >
            <option v-for="opt in PAGE_SIZE_OPTIONS" :key="opt" :value="opt">{{ opt }}</option>
          </select>
        </div>
      </div>
      <div class="overflow-x-auto">
        <table class="w-full border-collapse text-sm">
          <thead>
            <tr>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Vehicle</th>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">License plate</th>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Start</th>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">End</th>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Total</th>
              <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Status</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="r in items" :key="r.id" class="border-b border-gray-200">
              <td class="px-3 py-2">{{ r.vehicleModel }}</td>
              <td class="px-3 py-2">{{ r.licensePlate }}</td>
              <td class="px-3 py-2">{{ formatDate(r.startDate) }}</td>
              <td class="px-3 py-2">{{ formatDate(r.endDate) }}</td>
              <td class="px-3 py-2">{{ formatCurrency(r.totalPrice) }}</td>
              <td class="px-3 py-2">{{ r.status }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="totalPages > 1" class="mt-4 flex flex-wrap items-center justify-center gap-2">
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          :disabled="pageIndex <= 1"
          @click="goPage(pageIndex - 1)"
        >
          Previous
        </button>
        <div class="flex items-center gap-1">
          <template v-for="(p, i) in pageNumbers()" :key="i">
            <span v-if="p === -1" class="px-2 text-gray-400">…</span>
            <button
              v-else
              type="button"
              class="min-w-8 rounded-md border px-2 py-1 text-sm transition-colors"
              :class="p === pageIndex ? 'border-indigo-600 bg-indigo-50 text-indigo-700' : 'border-gray-300 hover:bg-gray-100'"
              @click="goPage(p)"
            >
              {{ p }}
            </button>
          </template>
        </div>
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          :disabled="pageIndex >= totalPages"
          @click="goPage(pageIndex + 1)"
        >
          Next
        </button>
      </div>
    </div>
  </div>
</template>
