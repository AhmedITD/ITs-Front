<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { vehiclesApi, type VehicleTypeDto } from '@/api/vehicles'

const types = ref<VehicleTypeDto[]>([])
const loading = ref(false)
const loadingList = ref(true)
const error = ref('')
const editingId = ref<number | null>(null)
const formName = ref('')
const formDescription = ref('')
const isCreate = ref(false)

async function load() {
  loadingList.value = true
  try {
    const { data } = await vehiclesApi.getTypes()
    if (data.success && data.data) types.value = data.data
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
  error.value = ''
  loading.value = true
  try {
    if (isCreate.value) {
      const { data } = await vehiclesApi.createType({ name: formName.value, description: formDescription.value || undefined })
      if (data.success) {
        await load()
        cancelEdit()
      } else {
        error.value = data.message || 'Failed to create'
      }
    } else if (editingId.value) {
      const { data } = await vehiclesApi.updateType(editingId.value, { name: formName.value, description: formDescription.value || undefined })
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
  if (!confirm('Delete this vehicle type?')) return
  loading.value = true
  error.value = ''
  try {
    const { data } = await vehiclesApi.deleteType(id)
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
    <h1>Vehicle types</h1>
    <div v-if="error" class="error">{{ error }}</div>
    <div class="form-row">
      <div v-if="editingId !== null || isCreate" class="inline-form">
        <input v-model="formName" type="text" placeholder="Name" required />
        <input v-model="formDescription" type="text" placeholder="Description (optional)" />
        <button type="button" class="btn btn-primary" :disabled="loading || !formName.trim()" @click="save">Save</button>
        <button type="button" class="btn" @click="cancelEdit">Cancel</button>
      </div>
      <button v-else type="button" class="btn btn-primary" @click="startCreate">Add type</button>
    </div>
    <div v-if="loadingList" class="loading">Loading…</div>
    <table v-else class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in types" :key="t.id">
          <td>{{ t.name }}</td>
          <td>{{ t.description || '—' }}</td>
          <td>
            <button type="button" class="btn btn-sm" @click="startEdit(t)">Edit</button>
            <button type="button" class="btn btn-sm btn-danger" @click="remove(t.id)">Delete</button>
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
.inline-form input[type="text"]:first-of-type {
  min-width: 160px;
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
