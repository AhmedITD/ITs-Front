import { ref } from 'vue'
import { useAmenityStore } from '@/stores/amenity'
import { useFormErrors } from '@/utils'
import type { AmenityDto } from '@/types/amenity'

const amenityStore = useAmenityStore()
const { setFromApi, get, clear, generalError } = useFormErrors()

const loading = ref(false)
const editingId = ref<number | null>(null)
const formName = ref('')
const formPrice = ref(0)
const isCreate = ref(false)

function initAmenitiesAdmin() {
  amenityStore.load()
  return () => {}
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
      await amenityStore.create({ name: formName.value, price: formPrice.value })
      cancelEdit()
    } else if (editingId.value) {
      await amenityStore.update(editingId.value, {
        name: formName.value,
        price: formPrice.value,
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
  if (!confirm('Delete this amenity?')) return
  loading.value = true
  clear()
  try {
    await amenityStore.remove(id)
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

// functions
export { initAmenitiesAdmin, startCreate, startEdit, cancelEdit, save, remove }
// stores
export { amenityStore }
// form state & errors
export { loading, editingId, formName, formPrice, isCreate, get, clear, generalError }
