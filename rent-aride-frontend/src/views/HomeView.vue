<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { vehiclesApi, type VehicleDto, type VehicleTypeDto } from '@/api/vehicles'

const router = useRouter()
const types = ref<VehicleTypeDto[]>([])
const vehicles = ref<VehicleDto[]>([])
const totalPages = ref(0)
const pageIndex = ref(1)
const pageSize = 10
const vehicleTypeId = ref<number | undefined>(undefined)
const loading = ref(false)
const loadingTypes = ref(true)

async function loadTypes() {
  loadingTypes.value = true
  try {
    const { data } = await vehiclesApi.getTypes()
    if (data.success && data.data) types.value = data.data
  } finally {
    loadingTypes.value = false
  }
}

async function loadVehicles() {
  loading.value = true
  try {
    const { data } = await vehiclesApi.browse({
      pageNumber: pageIndex.value,
      pageSize,
      vehicleTypeId: vehicleTypeId.value || undefined,
    })
    if (data.success && data.data) {
      vehicles.value = data.data.items
      totalPages.value = data.data.totalPages
    }
  } finally {
    loading.value = false
  }
}

function book(vehicle: VehicleDto) {
  router.push({ name: 'rental-new', query: { vehicleId: String(vehicle.id) } })
}

watch([pageIndex, vehicleTypeId], () => loadVehicles(), { immediate: true })
loadTypes()
</script>

<template>
  <div class="browse">
    <h1>Browse vehicles</h1>
    <div class="toolbar">
      <div class="filter">
        <label for="type">Vehicle type</label>
        <select id="type" v-model="vehicleTypeId" class="select">
          <option :value="undefined">All</option>
          <option v-for="t in types" :key="t.id" :value="t.id">{{ t.name }}</option>
        </select>
      </div>
    </div>
    <div v-if="loading" class="loading">Loading…</div>
    <div v-else-if="vehicles.length === 0" class="empty">No vehicles found.</div>
    <div v-else class="grid">
      <article v-for="v in vehicles" :key="v.id" class="card">
        <div class="card-body">
          <h3>{{ v.model }} ({{ v.year }})</h3>
          <p class="meta">{{ v.vehicleTypeName }} · {{ v.licensePlate }}</p>
          <p class="price">${{ v.dailyPrice.toFixed(2) }} <span class="unit">/ day</span></p>
          <p class="status">Status: {{ v.status }}</p>
          <button type="button" class="btn btn-primary" :disabled="v.status !== 'Available'" @click="book(v)">
            Book
          </button>
        </div>
      </article>
    </div>
    <div v-if="totalPages > 1" class="pagination">
      <button type="button" class="btn" :disabled="pageIndex <= 1" @click="pageIndex--">Previous</button>
      <span class="page-info">Page {{ pageIndex }} of {{ totalPages }}</span>
      <button type="button" class="btn" :disabled="pageIndex >= totalPages" @click="pageIndex++">Next</button>
    </div>
  </div>
</template>

<style scoped>
.browse h1 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}
.toolbar {
  margin-bottom: 1.5rem;
}
.filter label {
  margin-right: 0.5rem;
  font-size: 0.9rem;
}
.select {
  padding: 0.4rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  font-size: 0.95rem;
}
.loading,
.empty {
  padding: 2rem;
  text-align: center;
  color: var(--color-text);
}
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1rem;
}
.card {
  border: 1px solid var(--color-border);
  border-radius: 8px;
  overflow: hidden;
  background: var(--color-background-soft);
}
.card-body {
  padding: 1rem;
}
.card-body h3 {
  font-size: 1.1rem;
  margin-bottom: 0.25rem;
}
.meta {
  font-size: 0.9rem;
  color: var(--color-text);
  opacity: 0.9;
  margin-bottom: 0.5rem;
}
.price {
  font-size: 1.25rem;
  font-weight: 600;
  margin-bottom: 0.25rem;
}
.unit {
  font-size: 0.9rem;
  font-weight: 400;
  opacity: 0.8;
}
.status {
  font-size: 0.85rem;
  margin-bottom: 0.75rem;
}
.card .btn-primary {
  width: 100%;
  padding: 0.5rem;
  background: var(--vt-c-indigo);
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}
.card .btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.card .btn-primary:not(:disabled):hover {
  opacity: 0.9;
}
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
}
.page-info {
  font-size: 0.9rem;
}
</style>
