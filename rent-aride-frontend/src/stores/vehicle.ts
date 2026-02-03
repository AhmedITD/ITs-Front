import { defineStore } from 'pinia'
import { ref } from 'vue'
import { vehicleService } from '@/servers/services/VehicleService'
import type { VehicleDto, VehicleTypeDto } from '@/types/vehicle'

export const useVehicleStore = defineStore('vehicle', () => {
  const types = ref<VehicleTypeDto[]>([])
  const vehicles = ref<VehicleDto[]>([])
  const totalPages = ref(0)
  const totalCount = ref(0)
  const pageIndex = ref(1)
  const pageSize = ref(10)
  const vehicleTypeId = ref<number | undefined>(undefined)
  const vehicleStatus = ref<string | undefined>(undefined)
  const searchQuery = ref<string>('')
  const loading = ref(false)
  const loadingTypes = ref(true)
  const loadingList = ref(false)

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
        pageSize: pageSize.value,
        vehicleTypeId: vehicleTypeId.value ?? undefined,
        status: vehicleStatus.value ?? undefined,
        searchTerm: searchQuery.value?.trim() || undefined,
      })
      if (res.data) {
        vehicles.value = res.data.items
        totalPages.value = res.data.totalPages
        totalCount.value = res.data.totalCount
      }
    } finally {
      loading.value = false
    }
  }

  function setPageSize(size: number) {
    pageSize.value = size
  }

  function resetToFirstPage() {
    pageIndex.value = 1
  }

  async function loadAdminVehicles() {
    loadingList.value = true
    try {
      const [vRes, tRes] = await Promise.all([
        vehicleService.browse({ pageSize: 200 }),
        vehicleService.getTypes(),
      ])
      if (vRes.data) vehicles.value = vRes.data.items
      if (tRes.data) types.value = tRes.data
    } finally {
      loadingList.value = false
    }
  }

  async function createType(data: { name: string; description?: string }) {
    const res = await vehicleService.createType(data)
    if (!res.data) throw new Error(res.message || 'Failed to create')
    await loadTypes()
    return res
  }

  async function updateType(id: number, data: { name: string; description?: string }) {
    const res = await vehicleService.updateType(id, data)
    if (!res.data) throw new Error(res.message || 'Failed to update')
    await loadTypes()
    return res
  }

  async function deleteType(id: number) {
    await vehicleService.deleteType(id)
    await loadTypes()
  }

  async function createVehicle(data: {
    model: string
    year: number
    licensePlate: string
    dailyPrice: number
    vehicleTypeId: number
  }) {
    const res = await vehicleService.create(data)
    if (!res.data) throw new Error(res.message || 'Failed to create')
    await loadAdminVehicles()
    return res
  }

  async function updatePrice(id: number, dailyPrice: number) {
    const res = await vehicleService.updatePrice(id, dailyPrice)
    if (!res.data) throw new Error(res.message || 'Failed to update price')
    await loadAdminVehicles()
    return res
  }

  async function deleteVehicle(id: number) {
    await vehicleService.delete(id)
    await loadAdminVehicles()
  }

  return {
    types,
    vehicles,
    totalPages,
    totalCount,
    pageIndex,
    pageSize,
    vehicleTypeId,
    vehicleStatus,
    searchQuery,
    loading,
    loadingTypes,
    loadingList,
    loadTypes,
    loadVehicles,
    setPageSize,
    resetToFirstPage,
    loadAdminVehicles,
    createType,
    updateType,
    deleteType,
    createVehicle,
    updatePrice,
    deleteVehicle,
  }
})
