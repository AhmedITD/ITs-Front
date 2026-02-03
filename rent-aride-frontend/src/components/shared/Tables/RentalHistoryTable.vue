<script setup lang="ts">
import { formatCurrency } from '@/utils'
import type { RentalHistoryItemDto } from '@/types/rental'

const props = defineProps<{
  items: RentalHistoryItemDto[]
}>()

function formatDate(date: string) {
    return new Date(date).toLocaleDateString()
}

</script>
<template>
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
          <td class="px-3 py-2">
          <span
            :class="
              r.status === 'Active'
                ? 'font-medium text-emerald-600 dark:text-emerald-400'
                : 'font-medium text-rose-600 dark:text-rose-400'
            "
          >
            {{ r.status }}
          </span>
        </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>