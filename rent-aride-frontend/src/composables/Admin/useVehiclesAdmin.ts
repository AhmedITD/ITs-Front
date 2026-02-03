import { ref, watch } from 'vue'
import { useVehicleStore } from '@/stores/vehicle'
import { useFormErrors } from '@/utils'
import type { VehicleDto } from '@/types/vehicle'

const vehicleStore = useVehicleStore()
const { setFromApi, get, clear, generalError } = useFormErrors()

const loading = ref(false)
const showAdd = ref(false)
const formModel = ref('')
const formYear = ref(new Date().getFullYear())
const formLicensePlate = ref('')
const formDailyPrice = ref(0)
const formVehicleTypeId = ref<number | null>(null)
const editingPriceId = ref<number | null>(null)
const editingPrice = ref(0)

function initVehiclesAdmin() {
  vehicleStore.loadAdminVehicles()
  const stop = watch(
    () => vehicleStore.types,
    (t) => {
      if (t.length && formVehicleTypeId.value == null) formVehicleTypeId.value = t[0]?.id ?? null
    },
    { immediate: true }
  )
  return stop
}

function openAdd() {
  showAdd.value = true
  formModel.value = ''
  formYear.value = new Date().getFullYear()
  formLicensePlate.value = ''
  formDailyPrice.value = 0
  formVehicleTypeId.value = vehicleStore.types[0]?.id ?? null
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
    await vehicleStore.createVehicle({
      model,
      year: formYear.value,
      licensePlate: plate,
      dailyPrice: formDailyPrice.value,
      vehicleTypeId: formVehicleTypeId.value,
    })
    cancelAdd()
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
    await vehicleStore.updatePrice(editingPriceId.value, editingPrice.value)
    cancelEditPrice()
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
    await vehicleStore.deleteVehicle(id)
  } catch (e: unknown) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

// functions
export { initVehiclesAdmin, openAdd, cancelAdd, createVehicle, startEditPrice, cancelEditPrice, savePrice, remove }
// stores
export { vehicleStore }
// form state & errors
export { loading, showAdd, formModel, formYear, formLicensePlate, formDailyPrice, formVehicleTypeId, editingPriceId, editingPrice, get, clear, generalError }
