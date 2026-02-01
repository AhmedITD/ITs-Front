<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { vehicleService } from '@/servers/services/VehicleService'
import { useFormErrors } from '@/composables/useFormErrors'
import type { VehicleTypeDto } from '@/types/vehicle'

const { setFromApi, get, clear, generalError } = useFormErrors()
const types = ref<VehicleTypeDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const editingId = ref<number | null>(null)
const formName = ref('')
const formDescription = ref('')
const isCreate = ref(false)

async function load() {
  loadingList.value = true
  try {
    const res = await vehicleService.getTypes()
    if (res.data) types.value = res.data
  } finally {
    loadingList.value = false
  }
}

function startCreate() {
  isCreate.value = true
  editingId.value = null
  formName.value = ''
  formDescription.value = ''
}

function startEdit(t: VehicleTypeDto) {
  isCreate.value = false
  editingId.value = t.id
  formName.value = t.name
  formDescription.value = t.description || ''
}

function cancelEdit() {
  editingId.value = null
  isCreate.value = false
  formName.value = ''
  formDescription.value = ''
}

async function save() {
  clear()
  loading.value = true
  try {
    if (isCreate.value) {
      const res = await vehicleService.createType({
        name: formName.value,
        description: formDescription.value || undefined,
      })
      if (res.data) {
        await load()
        cancelEdit()
      } else {
        setFromApi({ message: res.message || 'Failed to create' })
      }
    } else if (editingId.value) {
      const res = await vehicleService.updateType(editingId.value, {
        name: formName.value,
        description: formDescription.value || undefined,
      })
      if (res.data) {
        await load()
        cancelEdit()
      } else {
        setFromApi({ message: res.message || 'Failed to update' })
      }
    }
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

async function remove(id: number) {
  if (!confirm('Delete this vehicle type?')) return
  loading.value = true
  clear()
  try {
    await vehicleService.deleteType(id)
    await load()
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

onMounted(load)
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Vehicle types</h1>
    <div v-if="generalError" class="rounded-md bg-red-50 p-3 text-sm text-red-700">{{ generalError }}</div>
    <div class="flex flex-wrap gap-2">
      <div v-if="editingId !== null || isCreate" class="flex flex-wrap items-end gap-2">
        <div>
          <input
            v-model="formName"
            type="text"
            placeholder="Name"
            required
            class="min-w-[160px] rounded-md border px-3 py-2"
            :class="get('name') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('name')" class="mt-1 text-sm text-red-600">{{ get('name') }}</p>
        </div>
        <div>
          <input
            v-model="formDescription"
            type="text"
            placeholder="Description (optional)"
            class="rounded-md border px-3 py-2"
            :class="get('description') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('description')" class="mt-1 text-sm text-red-600">{{ get('description') }}</p>
        </div>
        <button
          type="button"
          class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-60"
          :disabled="loading || !formName.trim()"
          @click="save"
        >
          Save
        </button>
        <button type="button" class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100" @click="cancelEdit">
          Cancel
        </button>
      </div>
      <button
        v-else
        type="button"
        class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700"
        @click="startCreate"
      >
        Add type
      </button>
    </div>
    <div v-if="loadingList" class="py-4 text-center">Loading…</div>
    <div v-else class="overflow-x-auto">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Name</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Description</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="t in types" :key="t.id" class="border-b border-gray-200">
            <td class="px-3 py-2">{{ t.name }}</td>
            <td class="px-3 py-2">{{ t.description || '—' }}</td>
            <td class="px-3 py-2">
              <button type="button" class="mr-2 rounded border px-2 py-1 text-sm hover:bg-gray-100" @click="startEdit(t)">
                Edit
              </button>
              <button
                type="button"
                class="rounded border border-red-200 px-2 py-1 text-sm text-red-700 hover:bg-red-50"
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
