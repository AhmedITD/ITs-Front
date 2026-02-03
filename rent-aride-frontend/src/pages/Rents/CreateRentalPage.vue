<script setup lang="ts">
import { onMounted } from 'vue'
import {
  vehicles,
  amenities,
  selectedVehicleId,
  startDate,
  endDate,
  selectedAmenityIds,
  loading,
  loadingForm,
  generalError,
  get,
  initCreateRental,
  onSubmit,
  toggleAmenity,
} from '@/composables/Rent/useCreateRental'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'
import { formatCurrency } from '@/utils'

onMounted(() => initCreateRental())
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold text-gray-900 dark:text-gray-100">Book a car</h1>
    <div v-if="loadingForm" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <form v-else class="max-w-md space-y-4 rounded-xl border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-600 dark:bg-gray-800" @submit.prevent="onSubmit">
      <div
        v-if="generalError && !get('vehicleId') && !get('startDate') && !get('endDate')"
        class="rounded-lg border border-rose-200 bg-rose-50 p-3 text-sm text-rose-700 dark:border-rose-800 dark:bg-rose-900/20 dark:text-rose-300"
      >
        {{ generalError }}
      </div>
      <div>
        <label class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Vehicle</label>
        <select
          v-model="selectedVehicleId"
          required
          class="w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
          :class="get('vehicleId') ? 'border-rose-500 dark:border-rose-500' : ''"
        >
          <option v-for="v in vehicles" :key="v.id" :value="v.id">
            {{ v.model }} ({{ v.year }}) — {{ formatCurrency(v.dailyPrice) }}/day
          </option>
        </select>
        <p v-if="get('vehicleId')" class="mt-1 text-sm text-rose-600 dark:text-rose-400">{{ get('vehicleId') }}</p>
      </div>
      <div>
        <label for="start" class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Start date</label>
        <input
          id="start"
          v-model="startDate"
          type="date"
          required
          class="w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
          :class="get('startDate') ? 'border-rose-500 dark:border-rose-500' : ''"
        />
        <p v-if="get('startDate')" class="mt-1 text-sm text-rose-600 dark:text-rose-400">{{ get('startDate') }}</p>
      </div>
      <div>
        <label for="end" class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">End date</label>
        <input
          id="end"
          v-model="endDate"
          type="date"
          required
          class="w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
          :class="get('endDate') ? 'border-rose-500 dark:border-rose-500' : ''"
        />
        <p v-if="get('endDate')" class="mt-1 text-sm text-rose-600 dark:text-rose-400">{{ get('endDate') }}</p>
      </div>
      <div>
        <label class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Extras</label>
        <div class="flex flex-wrap gap-4">
          <label v-for="a in amenities" :key="a.id" class="flex cursor-pointer items-center gap-2 text-sm text-gray-700 dark:text-gray-300">
            <input type="checkbox" class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" :checked="selectedAmenityIds.includes(a.id)" @change="toggleAmenity(a.id)" />
            {{ a.name }} <span class="font-medium text-indigo-600 dark:text-indigo-400">(+{{ formatCurrency(a.price) }})</span>
          </label>
        </div>
      </div>
      <div class="flex gap-3 pt-2">
        <router-link to="/" class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200 dark:hover:bg-gray-600">Cancel</router-link>
        <button
          type="submit"
          class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-60 dark:focus:ring-offset-gray-800"
          :disabled="loading"
        >
          {{ loading ? 'Redirecting to payment…' : 'Proceed to payment' }}
        </button>
      </div>
    </form>
  </div>
</template>
