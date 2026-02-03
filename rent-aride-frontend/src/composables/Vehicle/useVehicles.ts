import { watch } from 'vue'
import { useVehicleStore } from '@/stores/vehicle'
import type { VehicleDto } from '@/types/vehicle'

const vehicleStore = useVehicleStore()

function initVehicles(initialPageSize = 10) {
  vehicleStore.setPageSize(initialPageSize)
  vehicleStore.loadTypes()
  const stop = watch(
    () => [
      vehicleStore.pageIndex,
      vehicleStore.vehicleTypeId,
      vehicleStore.vehicleStatus,
      vehicleStore.searchQuery,
    ],
    () => vehicleStore.loadVehicles(),
    { immediate: true }
  )
  return stop
}
function book(vehicle: VehicleDto) {
  return { name: 'rental-new', query: { vehicleId: String(vehicle.id) } }
}
// stores
export { vehicleStore } 
// functions
export { initVehicles, book }
