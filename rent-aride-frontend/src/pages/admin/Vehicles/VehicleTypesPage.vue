<script setup lang="ts">
import { onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import {
  vehicleStore,
  initVehicleTypesAdmin,
  startCreate,
  startEdit,
  cancelEdit,
  save,
  remove,
  loading,
  editingId,
  formName,
  formDescription,
  isCreate,
  get,
  generalError,
} from '@/composables/Admin/useVehicleTypesAdmin'
import LoadingSpinner from '@/components/shared/LoadingSpinner.vue'

const { types, loadingTypes } = storeToRefs(vehicleStore)

onMounted(() => initVehicleTypesAdmin())
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Vehicle types</h1>
    <div v-if="generalError" class="rounded-lg border border-rose-200 bg-rose-50 p-3 text-sm text-rose-700 dark:border-rose-800 dark:bg-rose-900/20 dark:text-rose-300">{{ generalError }}</div>
    <div class="flex flex-wrap gap-2">
      <div v-if="editingId !== null || isCreate" class="flex flex-wrap items-end gap-2">
        <div>
          <input
            v-model="formName"
            type="text"
            placeholder="Name"
            required
            class="min-w-[160px] rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('name') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('name')" class="mt-1 text-sm text-red-600">{{ get('name') }}</p>
        </div>
        <div>
          <input
            v-model="formDescription"
            type="text"
            placeholder="Description (optional)"
            class="rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            :class="get('description') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('description')" class="mt-1 text-sm text-red-600">{{ get('description') }}</p>
        </div>
        <button
          type="button"
          class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-60 dark:focus:ring-offset-gray-800"
          :disabled="loading || !formName.trim()"
          @click="save"
        >
          Save
        </button>
        <button type="button" class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-200 dark:hover:bg-gray-700" @click="cancelEdit">
          Cancel
        </button>
      </div>
      <button
        v-else
        type="button"
        class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
        @click="startCreate"
      >
        Add type
      </button>
    </div>
    <div v-if="loadingTypes" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>
    <div v-else-if="types.length === 0" class="rounded-xl border border-gray-200 bg-white py-12 text-center text-gray-500 shadow-sm dark:border-gray-600 dark:bg-gray-800">
      No vehicle types yet. Add one above.
    </div>
    <div v-else class="overflow-x-auto rounded-xl border border-gray-200 bg-white shadow-sm dark:border-gray-600 dark:bg-gray-800">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Name</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100">Description</th>
            <th class="border-b border-gray-200 bg-gray-50 px-4 py-3 text-left font-semibold text-gray-900 dark:border-gray-600 dark:bg-gray-700/50 dark:text-gray-100"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="t in types" :key="t.id" class="border-b border-gray-200 last:border-b-0 dark:border-gray-600">
            <td class="px-4 py-3 font-medium text-gray-900 dark:text-gray-100">{{ t.name }}</td>
            <td class="px-4 py-3 text-gray-600 dark:text-gray-400">{{ t.description || '—' }}</td>
            <td class="px-4 py-3">
              <button type="button" class="mr-2 rounded-lg border border-gray-300 bg-white px-3 py-1.5 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-200 dark:hover:bg-gray-700" @click="startEdit(t)">
                Edit
              </button>
              <button
                type="button"
                class="rounded-lg border border-rose-200 px-3 py-1.5 text-sm font-medium text-rose-700 hover:bg-rose-50 dark:border-rose-800 dark:text-rose-300 dark:hover:bg-rose-900/20"
                @click="remove(t.id)"
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
