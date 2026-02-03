<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import { storeToRefs } from 'pinia'
import { vehicleStore, initVehicles , book } from '@/composables/Vehicle/useVehicles'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'
import Pagination from '@/components/shared/Pagination.vue'
import SercchBar from '@/components/shared/SercchBar.vue'
import StatusFilter from '@/components/shared/Filters/Vechile/StatusFilter.vue'
import TypeFilter from '@/components/shared/Filters/Vechile/TypeFilter.vue'
import type { VehicleDto } from '@/types/vehicle'
import VehicleCard from '@/components/shared/VehicleCard.vue'


const {
  types,
  vehicles,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  vehicleTypeId,
  vehicleStatus,
  searchQuery,
  loading,
} = storeToRefs(vehicleStore)

let stopWatcher: (() => void) | undefined
onMounted(() => {
  stopWatcher = initVehicles(10)
})
onUnmounted(() => {
  stopWatcher?.()
})


</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Browse vehicles</h1>
    <div class="flex flex-wrap items-end gap-4">
      <div class="min-w-[200px] max-w-xs">
        <SercchBar v-model="searchQuery" placeholder="Search vehicles…" />
      </div>
      <StatusFilter v-model="vehicleStatus" />
      <TypeFilter v-model="vehicleTypeId" :options="types" />
    </div>
    <div v-if="loading" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <div v-else-if="vehicles.length === 0" class="py-8 text-center text-gray-500">No vehicles found.</div>
    <div v-else class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      <VehicleCard 
        v-for="vehicle in vehicles" :key="vehicle.id" 
        :vehicle="vehicle"
        @book="book"
      />
    </div>
    <Pagination
      v-if="totalPages > 1"
      v-model:current-page="pageIndex"
      :total-pages="totalPages"
      :page-size="pageSize"
      :total-count="totalCount"
    />
  </div>
</template>
