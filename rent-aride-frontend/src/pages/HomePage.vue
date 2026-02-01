<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { vehicleService } from '@/servers/services/VehicleService'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'
import { formatCurrency } from '@/utils'
import type { VehicleDto, VehicleTypeDto } from '@/types/vehicle'

const router = useRouter()
const types = ref<VehicleTypeDto[]>([])
const vehicles = ref<VehicleDto[]>([])
const totalPages = ref(0)
const pageIndex = ref(1)
const pageSize = 10
const vehicleTypeId = ref<number | undefined>(undefined)
const loading = ref(false)
const loadingTypes = ref(true)

async function loadTypes() {
  loadingTypes.value = true
  try {
    const res = await vehicleService.getTypes()
    if (res.data) types.value = res.data
  } finally {
    loadingTypes.value = false
  }
}

async function loadVehicles() {
  loading.value = true
  try {
    const res = await vehicleService.browse({
      pageNumber: pageIndex.value,
      pageSize,
      vehicleTypeId: vehicleTypeId.value || undefined,
    })
    if (res.data) {
      vehicles.value = res.data.items
      totalPages.value = res.data.totalPages
    }
  } finally {
    loading.value = false
  }
}

function book(vehicle: VehicleDto) {
  router.push({ name: 'rental-new', query: { vehicleId: String(vehicle.id) } })
}

watch([pageIndex, vehicleTypeId], () => loadVehicles(), { immediate: true })
loadTypes()
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Browse vehicles</h1>
    <div class="flex gap-4">
      <div class="flex items-center gap-2">
        <label for="type" class="text-sm">Vehicle type</label>
        <select
          id="type"
          v-model="vehicleTypeId"
          class="rounded-md border border-gray-300 px-3 py-2 text-sm"
        >
          <option :value="undefined">All</option>
          <option v-for="t in types" :key="t.id" :value="t.id">{{ t.name }}</option>
        </select>
      </div>
    </div>
    <div v-if="loading" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <div v-else-if="vehicles.length === 0" class="py-8 text-center text-gray-500">No vehicles found.</div>
    <div v-else class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      <article
        v-for="v in vehicles"
        :key="v.id"
        class="rounded-lg border border-gray-200 bg-gray-50 p-4 dark:border-gray-700 dark:bg-gray-800"
      >
        <h3 class="font-medium">{{ v.model }} ({{ v.year }})</h3>
        <p class="text-sm text-gray-500">{{ v.vehicleTypeName }} · {{ v.licensePlate }}</p>
        <p class="mt-2 text-lg font-semibold">{{ formatCurrency(v.dailyPrice) }} <span class="text-sm font-normal">/ day</span></p>
        <p class="text-sm">Status: {{ v.status }}</p>
        <button
          type="button"
          class="mt-3 w-full rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
          :disabled="v.status !== 'Available'"
          @click="book(v)"
        >
          Book
        </button>
      </article>
    </div>
    <div v-if="totalPages > 1" class="flex items-center justify-center gap-4">
      <button
        type="button"
        class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
        :disabled="pageIndex <= 1"
        @click="pageIndex--"
      >
        Previous
      </button>
      <span class="text-sm">Page {{ pageIndex }} of {{ totalPages }}</span>
      <button
        type="button"
        class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
        :disabled="pageIndex >= totalPages"
        @click="pageIndex++"
      >
        Next
      </button>
    </div>
  </div>
</template>
