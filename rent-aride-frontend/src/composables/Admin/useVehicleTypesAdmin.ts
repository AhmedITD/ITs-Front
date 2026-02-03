import { ref } from 'vue'
import { useVehicleStore } from '@/stores/vehicle'
import { useFormErrors } from '@/utils'
import type { VehicleTypeDto } from '@/types/vehicle'

const vehicleStore = useVehicleStore()
const { setFromApi, get, clear, generalError } = useFormErrors()

const loading = ref(false)
const editingId = ref<number | null>(null)
const formName = ref('')
const formDescription = ref('')
const isCreate = ref(false)

function initVehicleTypesAdmin() {
  vehicleStore.loadTypes()
  return () => {}
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
      await vehicleStore.createType({
        name: formName.value,
        description: formDescription.value || undefined,
      })
      cancelEdit()
    } else if (editingId.value) {
      await vehicleStore.updateType(editingId.value, {
        name: formName.value,
        description: formDescription.value || undefined,
      })
      cancelEdit()
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
    await vehicleStore.deleteType(id)
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

// functions
export { initVehicleTypesAdmin, startCreate, startEdit, cancelEdit, save, remove }
// stores
export { vehicleStore }
// form state & errors
export { loading, editingId, formName, formDescription, isCreate, get, clear, generalError }
