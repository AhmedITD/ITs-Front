<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { vehicleService } from '@/servers/services/VehicleService'
import { useFormErrors } from '@/composables/useFormErrors'
import { formatCurrency } from '@/utils'
import type { VehicleDto, VehicleTypeDto } from '@/types/vehicle'

const { setFromApi, get, clear, generalError } = useFormErrors()
const vehicles = ref<VehicleDto[]>([])
const types = ref<VehicleTypeDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const showAdd = ref(false)
const formModel = ref('')
const formYear = ref(new Date().getFullYear())
const formLicensePlate = ref('')
const formDailyPrice = ref(0)
const formVehicleTypeId = ref<number | null>(null)
const editingPriceId = ref<number | null>(null)
const editingPrice = ref(0)

async function load() {
  loadingList.value = true
  try {
    const [vRes, tRes] = await Promise.all([
      vehicleService.browse({ pageSize: 200 }),
      vehicleService.getTypes(),
    ])
    if (vRes.data) vehicles.value = vRes.data.items
    if (tRes.data) {
      types.value = tRes.data
      if (types.value.length && !formVehicleTypeId.value) formVehicleTypeId.value = types.value[0]?.id ?? null
    }
  } finally {
    loadingList.value = false
  }
}

function openAdd() {
  showAdd.value = true
  formModel.value = ''
  formYear.value = new Date().getFullYear()
  formLicensePlate.value = ''
  formDailyPrice.value = 0
  formVehicleTypeId.value = types.value[0]?.id ?? null
}

function cancelAdd() {
  showAdd.value = false
}

async function createVehicle() {
  const model = String(formModel.value ?? '').trim()
  const plate = String(formLicensePlate.value ?? '').trim()
  if (!formVehicleTypeId.value || !model || !plate) {
    clear()
    generalError.value = 'Model, license plate and vehicle type are required.'
    return
  }
  clear()
  loading.value = true
  try {
    const res = await vehicleService.create({
      model,
      year: formYear.value,
      licensePlate: plate,
      dailyPrice: formDailyPrice.value,
      vehicleTypeId: formVehicleTypeId.value,
    })
    if (res.data) {
      await load()
      cancelAdd()
    } else {
      setFromApi({ message: res.message || 'Failed to create' })
    }
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

function startEditPrice(v: VehicleDto) {
  editingPriceId.value = v.id
  editingPrice.value = v.dailyPrice
}

function cancelEditPrice() {
  editingPriceId.value = null
}

async function savePrice() {
  if (editingPriceId.value == null) return
  clear()
  loading.value = true
  try {
    const res = await vehicleService.updatePrice(editingPriceId.value, editingPrice.value)
    if (res.data) {
      await load()
      cancelEditPrice()
    } else {
      setFromApi({ message: res.message || 'Failed to update price' })
    }
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

async function remove(id: number) {
  if (!confirm('Delete this vehicle?')) return
  loading.value = true
  clear()
  try {
    await vehicleService.delete(id)
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
    <h1 class="text-xl font-semibold">Vehicles</h1>
    <div v-if="generalError" class="rounded-md bg-red-50 p-3 text-sm text-red-700">{{ generalError }}</div>
    <div class="flex flex-wrap gap-2">
      <div v-if="showAdd" class="flex flex-wrap items-end gap-2">
        <div>
          <input
            v-model="formModel"
            type="text"
            placeholder="Model"
            class="min-w-[100px] rounded-md border px-3 py-2"
            :class="get('model') ? 'border-red-500' : 'border-gray-300'"
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
            class="w-24 rounded-md border px-3 py-2"
            :class="get('year') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('year')" class="mt-1 text-sm text-red-600">{{ get('year') }}</p>
        </div>
        <div>
          <input
            v-model="formLicensePlate"
            type="text"
            placeholder="License plate"
            class="rounded-md border px-3 py-2"
            :class="get('licensePlate') ? 'border-red-500' : 'border-gray-300'"
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
            class="w-24 rounded-md border px-3 py-2"
            :class="get('dailyPrice') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('dailyPrice')" class="mt-1 text-sm text-red-600">{{ get('dailyPrice') }}</p>
        </div>
        <div>
          <select
            v-model="formVehicleTypeId"
            class="rounded-md border px-3 py-2"
            :class="get('vehicleTypeId') ? 'border-red-500' : 'border-gray-300'"
          >
            <option v-for="t in types" :key="t.id" :value="t.id">{{ t.name }}</option>
          </select>
          <p v-if="get('vehicleTypeId')" class="mt-1 text-sm text-red-600">{{ get('vehicleTypeId') }}</p>
        </div>
        <button
          type="button"
          class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-60"
          :disabled="loading"
          @click="createVehicle"
        >
          Add vehicle
        </button>
        <button type="button" class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100" @click="cancelAdd">
          Cancel
        </button>
      </div>
      <button
        v-else
        type="button"
        class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700"
        @click="openAdd"
      >
        Add vehicle
      </button>
    </div>
    <div v-if="loadingList" class="py-4 text-center">Loading…</div>
    <div v-else class="overflow-x-auto">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Model</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Year</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">License plate</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Type</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Daily price</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold">Status</th>
            <th class="border-b border-gray-200 bg-gray-100 px-3 py-2 text-left font-semibold"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="v in vehicles" :key="v.id" class="border-b border-gray-200">
            <td class="px-3 py-2">{{ v.model }}</td>
            <td class="px-3 py-2">{{ v.year }}</td>
            <td class="px-3 py-2">{{ v.licensePlate }}</td>
            <td class="px-3 py-2">{{ v.vehicleTypeName }}</td>
            <td class="px-3 py-2">
              <template v-if="editingPriceId === v.id">
                <input
                  v-model.number="editingPrice"
                  type="number"
                  min="0"
                  step="0.01"
                  class="mr-2 w-20 rounded border px-2 py-1"
                />
                <button
                  type="button"
                  class="mr-1 rounded bg-indigo-600 px-2 py-1 text-sm text-white hover:bg-indigo-700 disabled:opacity-60"
                  :disabled="loading"
                  @click="savePrice"
                >
                  Save
                </button>
                <button type="button" class="rounded border px-2 py-1 text-sm hover:bg-gray-100" @click="cancelEditPrice">
                  Cancel
                </button>
              </template>
              <template v-else>
                {{ formatCurrency(v.dailyPrice) }}
                <button type="button" class="ml-2 rounded border px-2 py-1 text-sm hover:bg-gray-100" @click="startEditPrice(v)">
                  Edit
                </button>
              </template>
            </td>
            <td class="px-3 py-2">{{ v.status }}</td>
            <td class="px-3 py-2">
              <button
                type="button"
                class="rounded border border-red-200 px-2 py-1 text-sm text-red-700 hover:bg-red-50"
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
