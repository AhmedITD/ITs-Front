import { ref } from 'vue'
import type { RouteLocationNormalizedLoaded } from 'vue-router'
import { useFormErrors } from '@/utils'
import { amenityService } from '@/servers/services/AmenityService'
import { rentalService } from '@/servers/services/RentalService'
import { vehicleService } from '@/servers/services/VehicleService'
import type { VehicleDto } from '@/types/vehicle'
import type { AmenityDto } from '@/types/amenity'
import { useRoute } from 'vue-router'

const { setFromApi, get, clear, generalError } = useFormErrors()

const route = useRoute()
const vehicles = ref<VehicleDto[]>([])
const amenities = ref<AmenityDto[]>([])
const selectedVehicleId = ref<number | null>(null)
const startDate = ref('')
const endDate = ref('')
const selectedAmenityIds = ref<number[]>([])
const loading = ref(false)
const loadingForm = ref(true)

function toUtcIso(dateOnly: string, endOfDay: boolean): string {
  return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`
}

async function loadFormData(route?: RouteLocationNormalizedLoaded) {
  loadingForm.value = true
  try {
    const [vRes, aRes] = await Promise.all([
      vehicleService.browse({ pageSize: 500 }),
      amenityService.getAll(),
    ])
    if (vRes.data) vehicles.value = vRes.data.items.filter((v) => v.status === 'Available')
    if (aRes.data) amenities.value = aRes.data
    const vehicleIdFromQuery = route?.query.vehicleId ? Number(route.query.vehicleId) : null
    if (vehicleIdFromQuery && vehicles.value.some((v) => v.id === vehicleIdFromQuery)) {
      selectedVehicleId.value = vehicleIdFromQuery
    } else if (vehicles.value.length && selectedVehicleId.value == null) {
      selectedVehicleId.value = vehicles.value[0]?.id ?? null
    }
  } finally {
    loadingForm.value = false
  }
}

function initCreateRental() {
  const vehicleIdFromQuery = route?.query.vehicleId ? Number(route.query.vehicleId) : null
  selectedVehicleId.value = vehicleIdFromQuery
  loadFormData(route)
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

// functions
export { loadFormData, initCreateRental, onSubmit, toggleAmenity, toUtcIso, get, clear }
// state (refs)
export { vehicles, amenities, selectedVehicleId, startDate, endDate, selectedAmenityIds, loading, loadingForm, generalError }
