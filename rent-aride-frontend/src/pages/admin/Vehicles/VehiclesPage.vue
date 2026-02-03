<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import { storeToRefs } from 'pinia'
import {
  vehicleStore,
  initVehiclesAdmin,
  openAdd,
  cancelAdd,
  createVehicle,
  startEditPrice,
  cancelEditPrice,
  savePrice,
  remove,
  loading,
  showAdd,
  formModel,
  formYear,
  formLicensePlate,
  formDailyPrice,
  formVehicleTypeId,
  editingPriceId,
  editingPrice,
  get,
  generalError,
} from '@/composables/Admin/useVehiclesAdmin'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'
import { formatCurrency } from '@/utils'

const { types, vehicles, loadingList } = storeToRefs(vehicleStore)

let stopInit: (() => void) | undefined
onMounted(() => {
  stopInit = initVehiclesAdmin()
})
onUnmounted(() => {
  stopInit?.()
})
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Vehicles</h1>
    <div v-if="generalError" class="rounded-lg border border-rose-200 bg-rose-50 p-3 text-sm text-rose-700 dark:border-rose-800 dark:bg-rose-900/20 dark:text-rose-300">{{ generalError }}</div>
    <div class="flex flex-wrap gap-2">
      <div v-if="showAdd" class="flex flex-wrap items-end gap-2">
        <div>
          <input
            v-model="formModel"
            type="text"
            placeholder="Model"
            class="min-w-[100px] rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('model') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('model')" class="mt-1 text-sm text-red-600">{{ get('model') }}</p>
        </div>
        <div>
          <input
            v-model.number="formYear"
            type="number"
            min="1900"
            :max="new Date().getFullYear() + 1"
            placeholder="Year"
            class="w-24 rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('year') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('year')" class="mt-1 text-sm text-red-600">{{ get('year') }}</p>
        </div>
        <div>
          <input
            v-model="formLicensePlate"
            type="text"
            placeholder="License plate"
            class="rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('licensePlate') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('licensePlate')" class="mt-1 text-sm text-red-600">{{ get('licensePlate') }}</p>
        </div>
        <div>
          <input
            v-model.number="formDailyPrice"
            type="number"
            min="0"
            step="0.01"
            placeholder="Daily price"
            class="w-24 rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('dailyPrice') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('dailyPrice')" class="mt-1 text-sm text-red-600">{{ get('dailyPrice') }}</p>
        </div>
        <div>
          <select
            v-model="formVehicleTypeId"
            class="rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('vehicleTypeId') ? 'border-rose-500 dark:border-rose-500' : ''"
          >
            <option v-for="t in types" :key="t.id" :value="t.id">{{ t.name }}</option>
          </select>
          <p v-if="get('vehicleTypeId')" class="mt-1 text-sm text-red-600">{{ get('vehicleTypeId') }}</p>
        </div>
        <button
          type="button"
          class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-60 dark:focus:ring-offset-gray-800"
          :disabled="loading"
          @click="createVehicle"
        >
          Add vehicle
        </button>
        <button type="button" class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-200 dark:hover:bg-gray-700" @click="cancelAdd">
          Cancel
        </button>
      </div>
      <button
        v-else
        type="button"
        class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
        @click="openAdd"
      >
        Add vehicle
      </button>
    </div>
    <div v-if="loadingList" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <div v-else-if="vehicles.length === 0" class="rounded-xl border border-gray-200 bg-white py-12 text-center text-gray-500 shadow-sm dark:border-gray-600 dark:bg-gray-800">
      No vehicles yet. Add one above.
    </div>
    <div v-else class="overflow-x-auto rounded-xl border border-gray-200 bg-white shadow-sm dark:border-gray-600 dark:bg-gray-800">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Model</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Year</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">License plate</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Type</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Daily price</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Status</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="v in vehicles" :key="v.id" class="border-b border-gray-200 last:border-b-0 dark:border-gray-600">
            <td class="px-4 py-3 font-medium text-gray-900 dark:text-gray-100">{{ v.model }}</td>
            <td class="px-4 py-3 text-gray-600 dark:text-gray-400">{{ v.year }}</td>
            <td class="px-4 py-3 text-gray-600 dark:text-gray-400">{{ v.licensePlate }}</td>
            <td class="px-4 py-3 text-gray-600 dark:text-gray-400">{{ v.vehicleTypeName }}</td>
            <td class="px-4 py-3">
              <template v-if="editingPriceId === v.id">
                <input
                  v-model.number="editingPrice"
                  type="number"
                  min="0"
                  step="0.01"
                  class="mr-2 w-24 rounded-lg border border-gray-300 px-2 py-1.5 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
                />
                <button
                  type="button"
                  class="mr-1 rounded-lg bg-indigo-600 px-2.5 py-1.5 text-sm font-medium text-white hover:bg-indigo-700 disabled:opacity-60"
                  :disabled="loading"
                  @click="savePrice"
                >
                  Save
                </button>
                <button type="button" class="rounded-lg border border-gray-300 px-2.5 py-1.5 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-200 dark:hover:bg-gray-700" @click="cancelEditPrice">
                  Cancel
                </button>
              </template>
              <template v-else>
                <span class="font-medium text-indigo-600 dark:text-indigo-400">{{ formatCurrency(v.dailyPrice) }}</span>
                <button type="button" class="ml-2 rounded-lg border border-gray-300 bg-white px-2.5 py-1.5 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200 dark:hover:bg-gray-600" @click="startEditPrice(v)">
                  Edit
                </button>
              </template>
            </td>
            <td class="px-4 py-3">
              <span
                :class="
                  v.status === 'Available'
                    ? 'font-medium text-emerald-600 dark:text-emerald-400'
                    : 'font-medium text-rose-600 dark:text-rose-400'
                "
              >
                {{ v.status }}
              </span>
            </td>
            <td class="px-4 py-3">
              <button
                type="button"
                class="rounded-lg border border-rose-200 px-3 py-1.5 text-sm font-medium text-rose-700 hover:bg-rose-50 dark:border-rose-800 dark:text-rose-300 dark:hover:bg-rose-900/20"
                @click="remove(v.id)"
              >
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
