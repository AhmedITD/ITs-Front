<script setup lang="ts">
import { formatCurrency } from '@/utils'
import type { VehicleDto } from '@/types/vehicle'

defineEmits<{
  (e: 'book', vehicle: VehicleDto): void
}>()
defineProps<{
  vehicle: VehicleDto
}>()
</script>

<template>
    <article
    :class="
        vehicle.status === 'Available'
            ? 'rounded-xl border border-gray-200 bg-white p-5 shadow-sm transition-shadow hover:shadow-md dark:border-gray-600 dark:bg-gray-800'
            : 'rounded-xl border border-gray-200 bg-white p-5 shadow-sm transition-shadow hover:shadow-md dark:border-gray-600 dark:bg-gray-800'
    "
    >
    <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-100">{{ vehicle.model }} ({{ vehicle.year }})</h3>
    <p class="mt-1 text-sm text-gray-600 dark:text-gray-400">{{ vehicle.vehicleTypeName }} · {{ vehicle.licensePlate }}</p>
    <p class="mt-3 text-xl font-bold text-indigo-600 dark:text-indigo-400">
    {{ formatCurrency(vehicle.dailyPrice) }}
    <span class="text-sm font-normal text-gray-500 dark:text-gray-400">/ day</span>
    </p>
    <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
    Status:
    <span
        :class="
        vehicle.status === 'Available'
            ? 'font-medium text-emerald-600 dark:text-emerald-400'
            : 'font-medium text-rose-600 dark:text-rose-400'
        "
    >
        {{ vehicle.status }}
    </span>
    </p>
    <button
    type="button"
    class="mt-4 w-full rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed dark:focus:ring-offset-gray-800"
    :disabled="vehicle.status !== 'Available'"
    @click="$emit('book', vehicle)"
    >
    Book
    </button>
    </article>
</template>