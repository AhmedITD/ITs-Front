import { ref, watch } from 'vue'
import { vehicleService } from '@/servers/services/VehicleService'
import type { VehicleDto, VehicleTypeDto } from '@/types/vehicle'

export function useVehicles(pageSize = 10) {
  const types = ref<VehicleTypeDto[]>([])
  const vehicles = ref<VehicleDto[]>([])
  const totalPages = ref(0)
  const pageIndex = ref(1)
  const vehicleTypeId = ref<number | undefined>(undefined)
  const loading = ref(false)
  const loadingTypes = ref(true)

  async function loadTypes() {
    loadingTypes.value = true
    try {
    const res = await vehicleService.getTypes()
    if (res.data) types.value = res.data
    } finally {
      loadingTypes.value = false
    }
  }

  async function loadVehicles() {
    loading.value = true
    try {
      const res = await vehicleService.browse({
        pageNumber: pageIndex.value,
        pageSize,
        vehicleTypeId: vehicleTypeId.value || undefined,
      })
      if (res.data) {
        vehicles.value = res.data.items
        totalPages.value = res.data.totalPages
      }
    } finally {
      loading.value = false
    }
  }

  watch(
    [pageIndex, vehicleTypeId],
    () => loadVehicles(),
    { immediate: true }
  )

  return {
    types,
    vehicles,
    totalPages,
    pageIndex,
    vehicleTypeId,
    loading,
    loadingTypes,
    loadTypes,
    loadVehicles,
  }
}
