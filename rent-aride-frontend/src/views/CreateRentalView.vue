<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { vehiclesApi, type VehicleDto } from '@/api/vehicles'
import { amenitiesApi, type AmenityDto } from '@/api/amenities'
import { rentalsApi } from '@/api/rentals'

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
const error = ref('')
const loading = ref(false)
const loadingForm = ref(true)

async function loadFormData() {
  loadingForm.value = true
  try {
    const [vRes, aRes] = await Promise.all([
      vehiclesApi.browse({ pageSize: 500 }),
      amenitiesApi.getAll(),
    ])
    if (vRes.data.success && vRes.data.data) vehicles.value = vRes.data.data.items.filter((v) => v.status === 'Available')
    if (aRes.data.success && aRes.data.data) amenities.value = aRes.data.data
    if (vehicleIdFromQuery.value && vehicles.value.some((v) => v.id === vehicleIdFromQuery.value)) {
      selectedVehicleId.value = vehicleIdFromQuery.value
    } else if (vehicles.value.length && !selectedVehicleId.value) {
      selectedVehicleId.value = vehicles.value[0]?.id ?? null as number | null
    }
  } finally {
    loadingForm.value = false
  }
}

/** Send date as ISO 8601 UTC so backend receives DateTime with Kind=Utc (PostgreSQL timestamptz). */
function toUtcIso(dateOnly: string, endOfDay: boolean): string {
  return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`
}

async function onSubmit() {
  if (!selectedVehicleId.value || !startDate.value || !endDate.value) {
    error.value = 'Please select a vehicle and dates.'
    return
  }
  error.value = ''
  loading.value = true
  try {
    const { data } = await rentalsApi.create({
      vehicleId: selectedVehicleId.value,
      startDate: toUtcIso(startDate.value, false),
      endDate: toUtcIso(endDate.value, true),
      amenityIds: selectedAmenityIds.value,
    })
    if (data.success && data.data) {
      await router.push({ name: 'rentals' })
    } else {
      error.value = data.message || 'Failed to create rental'
    }
  } catch (e: unknown) {
    const ax = e as { response?: { data?: { message?: string; errors?: Record<string, string[]> } } }
    const msg = ax.response?.data?.message
    const errs = ax.response?.data?.errors
    if (errs && Object.keys(errs).length) {
      error.value = Object.entries(errs)
        .map(([k, v]) => `${k}: ${(v as string[]).join(', ')}`)
        .join('; ')
    } else {
      error.value = msg || 'Failed to create rental'
    }
  } finally {
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
  <div class="create-rental">
    <h1>Book a car</h1>
    <div v-if="loadingForm" class="loading">Loading…</div>
    <form v-else class="form" @submit.prevent="onSubmit">
      <div v-if="error" class="error">{{ error }}</div>
      <div class="field">
        <label>Vehicle</label>
        <select v-model="selectedVehicleId" required class="select">
          <option v-for="v in vehicles" :key="v.id" :value="v.id">
            {{ v.model }} ({{ v.year }}) — ${{ v.dailyPrice.toFixed(2) }}/day
          </option>
        </select>
      </div>
      <div class="field">
        <label for="start">Start date</label>
        <input id="start" v-model="startDate" type="date" required />
      </div>
      <div class="field">
        <label for="end">End date</label>
        <input id="end" v-model="endDate" type="date" required />
      </div>
      <div class="field">
        <label>Extras</label>
        <div class="amenities">
          <label v-for="a in amenities" :key="a.id" class="checkbox-label">
            <input type="checkbox" :checked="selectedAmenityIds.includes(a.id)" @change="toggleAmenity(a.id)" />
            {{ a.name }} (+${{ a.price.toFixed(2) }})
          </label>
        </div>
      </div>
      <div class="actions">
        <router-link to="/" class="btn">Cancel</router-link>
        <button type="submit" class="btn btn-primary" :disabled="loading">
          {{ loading ? 'Booking…' : 'Book' }}
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.create-rental h1 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}
.loading {
  padding: 2rem;
  text-align: center;
}
.form {
  max-width: 420px;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
.error {
  padding: 0.5rem 0.75rem;
  background: #fef2f2;
  color: #b91c1c;
  border-radius: 6px;
  font-size: 0.9rem;
}
.field label {
  display: block;
  margin-bottom: 0.25rem;
  font-size: 0.9rem;
  font-weight: 500;
}
.field input[type="date"],
.select {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  font-size: 1rem;
}
.amenities {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
}
.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.35rem;
  font-weight: 400;
  cursor: pointer;
}
.actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
}
.btn {
  padding: 0.5rem 1rem;
  border-radius: 6px;
  text-decoration: none;
  border: 1px solid var(--color-border);
  background: var(--color-background-soft);
  color: var(--color-text);
  font-size: 0.95rem;
  cursor: pointer;
}
.btn-primary {
  background: var(--vt-c-indigo);
  color: white;
  border: none;
}
.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
