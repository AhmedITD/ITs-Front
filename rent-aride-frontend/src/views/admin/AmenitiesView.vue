<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { amenitiesApi, type AmenityDto } from '@/api/amenities'

const amenities = ref<AmenityDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const error = ref('')
const editingId = ref<number | null>(null)
const formName = ref('')
const formPrice = ref<number>(0)
const isCreate = ref(false)

async function load() {
  loadingList.value = true
  try {
    const { data } = await amenitiesApi.getAll()
    if (data.success && data.data) amenities.value = data.data
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
  error.value = ''
  loading.value = true
  try {
    if (isCreate.value) {
      const { data } = await amenitiesApi.create({ name: formName.value, price: formPrice.value })
      if (data.success) {
        await load()
        cancelEdit()
      } else {
        error.value = data.message || 'Failed to create'
      }
    } else if (editingId.value) {
      const { data } = await amenitiesApi.update(editingId.value, { name: formName.value, price: formPrice.value })
      if (data.success) {
        await load()
        cancelEdit()
      } else {
        error.value = data.message || 'Failed to update'
      }
    }
  } catch (e: unknown) {
    const ax = e as { response?: { data?: { message?: string } } }
    error.value = ax.response?.data?.message || 'Request failed'
  } finally {
    loading.value = false
  }
}

async function remove(id: number) {
  if (!confirm('Delete this amenity?')) return
  loading.value = true
  error.value = ''
  try {
    const { data } = await amenitiesApi.delete(id)
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
    <h1>Amenities</h1>
    <div v-if="error" class="error">{{ error }}</div>
    <div class="form-row">
      <div v-if="editingId !== null || isCreate" class="inline-form">
        <input v-model="formName" type="text" placeholder="Name" required />
        <input v-model.number="formPrice" type="number" min="0" step="0.01" placeholder="Price" />
        <button type="button" class="btn btn-primary" :disabled="loading || !formName.trim()" @click="save">Save</button>
        <button type="button" class="btn" @click="cancelEdit">Cancel</button>
      </div>
      <button v-else type="button" class="btn btn-primary" @click="startCreate">Add amenity</button>
    </div>
    <div v-if="loadingList" class="loading">Loading…</div>
    <table v-else class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Price</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="a in amenities" :key="a.id">
          <td>{{ a.name }}</td>
          <td>${{ a.price.toFixed(2) }}</td>
          <td>
            <button type="button" class="btn btn-sm" @click="startEdit(a)">Edit</button>
            <button type="button" class="btn btn-sm btn-danger" @click="remove(a.id)">Delete</button>
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
.inline-form {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  align-items: center;
}
.inline-form input {
  padding: 0.4rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  font-size: 0.95rem;
}
.inline-form input[type="text"] {
  min-width: 140px;
}
.inline-form input[type="number"] {
  width: 90px;
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
