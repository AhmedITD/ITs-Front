<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { vehicleService } from '@/servers/services/VehicleService'
import { amenityService } from '@/servers/services/AmenityService'
import { rentalService } from '@/servers/services/RentalService'
import { useFormErrors } from '@/composables/useFormErrors'
import { formatCurrency } from '@/utils'
import type { VehicleDto } from '@/types/vehicle'
import type { AmenityDto } from '@/types/amenity'

const { setFromApi, get, clear, generalError } = useFormErrors()

const route = useRoute()
const router = useRouter()
const vehicleIdFromQuery = computed(() => {
  const id = route.query.vehicleId
  return id ? Number(id) : null
})

const vehicles = ref<VehicleDto[]>([])
const amenities = ref<AmenityDto[]>([])
const selectedVehicleId = ref<number | null>(vehicleIdFromQuery.value)
const startDate = ref('')
const endDate = ref('')
const selectedAmenityIds = ref<number[]>([])
const loading = ref(false)
const loadingForm = ref(true)

async function loadFormData() {
  loadingForm.value = true
  try {
    const [vRes, aRes] = await Promise.all([
      vehicleService.browse({ pageSize: 500 }),
      amenityService.getAll(),
    ])
    if (vRes.data) vehicles.value = vRes.data.items.filter((v) => v.status === 'Available')
    if (aRes.data) amenities.value = aRes.data
    if (vehicleIdFromQuery.value && vehicles.value.some((v) => v.id === vehicleIdFromQuery.value)) {
      selectedVehicleId.value = vehicleIdFromQuery.value
    } else if (vehicles.value.length && !selectedVehicleId.value) {
      selectedVehicleId.value = vehicles.value[0]?.id ?? null
    }
  } finally {
    loadingForm.value = false
  }
}

function toUtcIso(dateOnly: string, endOfDay: boolean): string {
  return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`
}

async function onSubmit() {
  if (!selectedVehicleId.value || !startDate.value || !endDate.value) {
    clear()
    generalError.value = 'Please select a vehicle and dates.'
    return
  }
  clear()
  loading.value = true
  try {
    const finishUrl = `${window.location.origin}/payment/finish`
    const res = await rentalService.createInvoice({
      vehicleId: selectedVehicleId.value,
      startDate: toUtcIso(startDate.value, false),
      endDate: toUtcIso(endDate.value, true),
      amenityIds: selectedAmenityIds.value,
      finishUrl,
    })
    if (res.data?.paymentUrl) {
      window.location.href = res.data.paymentUrl
    } else {
      generalError.value = res.message || 'Failed to create payment link'
      loading.value = false
    }
  } catch (e: unknown) {
    setFromApi(e)
    loading.value = false
  }
}

function toggleAmenity(id: number) {
  const i = selectedAmenityIds.value.indexOf(id)
  if (i === -1) selectedAmenityIds.value = [...selectedAmenityIds.value, id]
  else selectedAmenityIds.value = selectedAmenityIds.value.filter((x) => x !== id)
}

onMounted(loadFormData)
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Book a car</h1>
    <div v-if="loadingForm" class="py-8 text-center">Loading…</div>
    <form v-else class="flex max-w-md flex-col gap-4" @submit.prevent="onSubmit">
      <div
        v-if="generalError && !get('vehicleId') && !get('startDate') && !get('endDate')"
        class="rounded-md bg-red-50 p-3 text-sm text-red-700"
      >
        {{ generalError }}
      </div>
      <div>
        <label class="mb-1 block text-sm font-medium">Vehicle</label>
        <select
          v-model="selectedVehicleId"
          required
          class="w-full rounded-md border px-3 py-2"
          :class="get('vehicleId') ? 'border-red-500' : 'border-gray-300'"
        >
          <option v-for="v in vehicles" :key="v.id" :value="v.id">
            {{ v.model }} ({{ v.year }}) — {{ formatCurrency(v.dailyPrice) }}/day
          </option>
        </select>
        <p v-if="get('vehicleId')" class="mt-1 text-sm text-red-600">{{ get('vehicleId') }}</p>
      </div>
      <div>
        <label for="start" class="mb-1 block text-sm font-medium">Start date</label>
        <input
          id="start"
          v-model="startDate"
          type="date"
          required
          class="w-full rounded-md border px-3 py-2"
          :class="get('startDate') ? 'border-red-500' : 'border-gray-300'"
        />
        <p v-if="get('startDate')" class="mt-1 text-sm text-red-600">{{ get('startDate') }}</p>
      </div>
      <div>
        <label for="end" class="mb-1 block text-sm font-medium">End date</label>
        <input
          id="end"
          v-model="endDate"
          type="date"
          required
          class="w-full rounded-md border px-3 py-2"
          :class="get('endDate') ? 'border-red-500' : 'border-gray-300'"
        />
        <p v-if="get('endDate')" class="mt-1 text-sm text-red-600">{{ get('endDate') }}</p>
      </div>
      <div>
        <label class="mb-1 block text-sm font-medium">Extras</label>
        <div class="flex flex-wrap gap-4">
          <label v-for="a in amenities" :key="a.id" class="flex cursor-pointer items-center gap-2">
            <input type="checkbox" :checked="selectedAmenityIds.includes(a.id)" @change="toggleAmenity(a.id)" />
            {{ a.name }} (+{{ formatCurrency(a.price) }})
          </label>
        </div>
      </div>
      <div class="flex gap-3">
        <router-link to="/" class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100">Cancel</router-link>
        <button
          type="submit"
          class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-60"
          :disabled="loading"
        >
          {{ loading ? 'Redirecting to payment…' : 'Proceed to payment' }}
        </button>
      </div>
    </form>
  </div>
</template>
