<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { vehiclesApi, type VehicleDto, type VehicleTypeDto } from '@/api/vehicles'

const vehicles = ref<VehicleDto[]>([])
const types = ref<VehicleTypeDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const error = ref('')
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
      vehiclesApi.browse({ pageSize: 200 }),
      vehiclesApi.getTypes(),
    ])
    if (vRes.data.success && vRes.data.data) vehicles.value = vRes.data.data.items
    if (tRes.data.success && tRes.data.data) {
      types.value = tRes.data.data
      if (types.value.length && !formVehicleTypeId.value) formVehicleTypeId.value = types.value[0].id
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
    error.value = 'Model, license plate and vehicle type are required.'
    return
  }
  error.value = ''
  loading.value = true
  try {
    const { data } = await vehiclesApi.create({
      model,
      year: formYear.value,
      licensePlate: plate,
      dailyPrice: formDailyPrice.value,
      vehicleTypeId: formVehicleTypeId.value,
    })
    if (data.success) {
      await load()
      cancelAdd()
    } else {
      error.value = data.message || 'Failed to create'
    }
  } catch (e: unknown) {
    const ax = e as { response?: { data?: { message?: string } } }
    error.value = ax.response?.data?.message || 'Request failed'
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
  error.value = ''
  loading.value = true
  try {
    const { data } = await vehiclesApi.updatePrice(editingPriceId.value, editingPrice.value)
    if (data.success) {
      await load()
      cancelEditPrice()
    } else {
      error.value = data.message || 'Failed to update price'
    }
  } catch (e: unknown) {
    const ax = e as { response?: { data?: { message?: string } } }
    error.value = ax.response?.data?.message || 'Request failed'
  } finally {
    loading.value = false
  }
}

async function remove(id: number) {
  if (!confirm('Delete this vehicle?')) return
  loading.value = true
  error.value = ''
  try {
    const { data } = await vehiclesApi.delete(id)
    if (data.success) await load()
    else error.value = data.message || 'Failed to delete'
  } catch (e: unknown) {
    const ax = e as { response?: { data?: { message?: string } } }
    error.value = ax.response?.data?.message || 'Request failed'
  } finally {
    loading.value = false
  }
}

onMounted(load)
</script>

<template>
  <div class="admin-page">
    <h1>Vehicles</h1>
    <div v-if="error" class="error">{{ error }}</div>
    <div class="form-row">
      <div v-if="showAdd" class="add-form">
        <input v-model="formModel" type="text" placeholder="Model" />
        <input v-model.number="formYear" type="number" min="1900" :max="new Date().getFullYear() + 1" placeholder="Year" />
        <input v-model="formLicensePlate" type="text" placeholder="License plate" />
        <input v-model.number="formDailyPrice" type="number" min="0" step="0.01" placeholder="Daily price" />
        <select v-model="formVehicleTypeId">
          <option v-for="t in types" :key="t.id" :value="t.id">{{ t.name }}</option>
        </select>
        <button type="button" class="btn btn-primary" :disabled="loading" @click="createVehicle">Add vehicle</button>
        <button type="button" class="btn" @click="cancelAdd">Cancel</button>
      </div>
      <button v-else type="button" class="btn btn-primary" @click="openAdd">Add vehicle</button>
    </div>
    <div v-if="loadingList" class="loading">Loading…</div>
    <table v-else class="table">
      <thead>
        <tr>
          <th>Model</th>
          <th>Year</th>
          <th>License plate</th>
          <th>Type</th>
          <th>Daily price</th>
          <th>Status</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="v in vehicles" :key="v.id">
          <td>{{ v.model }}</td>
          <td>{{ v.year }}</td>
          <td>{{ v.licensePlate }}</td>
          <td>{{ v.vehicleTypeName }}</td>
          <td>
            <template v-if="editingPriceId === v.id">
              <input v-model.number="editingPrice" type="number" min="0" step="0.01" class="price-input" />
              <button type="button" class="btn btn-sm btn-primary" :disabled="loading" @click="savePrice">Save</button>
              <button type="button" class="btn btn-sm" @click="cancelEditPrice">Cancel</button>
            </template>
            <template v-else>
              ${{ v.dailyPrice.toFixed(2) }}
              <button type="button" class="btn btn-sm" @click="startEditPrice(v)">Edit</button>
            </template>
          </td>
          <td>{{ v.status }}</td>
          <td>
            <button type="button" class="btn btn-sm btn-danger" @click="remove(v.id)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.admin-page h1 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}
.error {
  padding: 0.5rem 0.75rem;
  background: #fef2f2;
  color: #b91c1c;
  border-radius: 6px;
  margin-bottom: 1rem;
  font-size: 0.9rem;
}
.form-row {
  margin-bottom: 1rem;
}
.add-form {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  align-items: center;
}
.add-form input,
.add-form select {
  padding: 0.4rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  font-size: 0.95rem;
}
.add-form input {
  min-width: 100px;
}
.add-form .price-input {
  width: 80px;
}
.loading {
  padding: 1rem;
  text-align: center;
}
.table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}
.table th,
.table td {
  padding: 0.6rem 0.75rem;
  text-align: left;
  border-bottom: 1px solid var(--color-border);
}
.table th {
  font-weight: 600;
  background: var(--color-background-mute);
}
.btn {
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  border: 1px solid var(--color-border);
  background: var(--color-background-soft);
  color: var(--color-text);
  font-size: 0.9rem;
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
.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.85rem;
  margin-right: 0.25rem;
}
.btn-danger {
  color: #b91c1c;
  border-color: #fecaca;
}
.btn-danger:hover {
  background: #fef2f2;
}
</style>
