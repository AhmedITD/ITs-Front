<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { amenityService } from '@/servers/services/AmenityService'
import { useFormErrors } from '@/composables/useFormErrors'
import { formatCurrency } from '@/utils'
import type { AmenityDto } from '@/types/amenity'

const { setFromApi, get, clear, generalError } = useFormErrors()
const amenities = ref<AmenityDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const editingId = ref<number | null>(null)
const formName = ref('')
const formPrice = ref(0)
const isCreate = ref(false)

async function load() {
  loadingList.value = true
  try {
    const res = await amenityService.getAll()
    if (res.data) amenities.value = res.data
  } finally {
    loadingList.value = false
  }
}

function startCreate() {
  isCreate.value = true
  editingId.value = null
  formName.value = ''
  formPrice.value = 0
}

function startEdit(a: AmenityDto) {
  isCreate.value = false
  editingId.value = a.id
  formName.value = a.name
  formPrice.value = a.price
}

function cancelEdit() {
  editingId.value = null
  isCreate.value = false
  formName.value = ''
  formPrice.value = 0
}

async function save() {
  clear()
  loading.value = true
  try {
    if (isCreate.value) {
      const res = await amenityService.create({ name: formName.value, price: formPrice.value })
      if (res.data) {
        await load()
        cancelEdit()
      } else {
        setFromApi({ message: res.message || 'Failed to create' })
      }
    } else if (editingId.value) {
      const res = await amenityService.update(editingId.value, {
        name: formName.value,
        price: formPrice.value,
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
  if (!confirm('Delete this amenity?')) return
  loading.value = true
  clear()
  try {
    await amenityService.delete(id)
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
    <h1 class="text-xl font-semibold">Amenities</h1>
    <div v-if="generalError" class="rounded-md bg-red-50 p-3 text-sm text-red-700">{{ generalError }}</div>
    <div class="flex flex-wrap gap-2">
      <div v-if="editingId !== null || isCreate" class="flex flex-wrap items-end gap-2">
        <div>
          <input
            v-model="formName"
            type="text"
            placeholder="Name"
            required
            class="min-w-[140px] rounded-md border px-3 py-2"
            :class="get('name') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('name')" class="mt-1 text-sm text-red-600">{{ get('name') }}</p>
        </div>
        <div>
          <input
            v-model.number="formPrice"
            type="number"
            min="0"
            step="0.01"
            placeholder="Price"
            class="w-24 rounded-md border px-3 py-2"
            :class="get('price') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('price')" class="mt-1 text-sm text-red-600">{{ get('price') }}</p>
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
        Add amenity
      </button>
    </div>
    <div v-if="loadingList" class="py-4 text-center">Loading…</div>
    <div v-else class="overflow-x-auto">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Name</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Price</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="a in amenities" :key="a.id" class="border-b border-gray-200">
            <td class="px-3 py-2">{{ a.name }}</td>
            <td class="px-3 py-2">{{ formatCurrency(a.price) }}</td>
            <td class="px-3 py-2">
              <button type="button" class="mr-2 rounded border px-2 py-1 text-sm hover:bg-gray-100" @click="startEdit(a)">
                Edit
              </button>
              <button
                type="button"
                class="rounded border border-red-200 px-2 py-1 text-sm text-red-700 hover:bg-red-50"
                @click="remove(a.id)"
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
