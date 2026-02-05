import { writable, get } from 'svelte/store'
import { vehicleService } from '../servers/services/VehicleService.js'
import type { VehicleDto, VehicleTypeDto } from '../types/vehicle.js'

export const types = writable<VehicleTypeDto[]>([])
export const vehicles = writable<VehicleDto[]>([])
export const totalPages = writable(0)
export const totalCount = writable(0)
export const pageIndex = writable(1)
export const pageSize = writable(10)
export const vehicleTypeId = writable<number | string | undefined>(undefined)
export const vehicleStatus = writable<string | undefined>(undefined)
export const searchQuery = writable('')
export const loading = writable(false)
export const loadingTypes = writable(true)

export async function loadTypes(): Promise<void> {
  loadingTypes.set(true)
  try {
    const res = await vehicleService.getTypes()
    if (res.data) types.set(res.data)
  } catch {
    types.set([])
  } finally {
    loadingTypes.set(false)
  }
}

export async function loadVehicles(): Promise<void> {
  loading.set(true)
  try {
    const res = await vehicleService.browse({
      pageNumber: get(pageIndex),
      pageSize: get(pageSize),
      vehicleTypeId: (() => {
        const v = get(vehicleTypeId)
        return v === '' || v == null ? undefined : Number(v)
      })(),
      status: get(vehicleStatus) ?? undefined,
      searchTerm: get(searchQuery)?.trim() || undefined,
    })
    if (res.data) {
      vehicles.set(res.data.items)
      totalPages.set(res.data.totalPages)
      totalCount.set(res.data.totalCount)
    }
  } catch {
    vehicles.set([])
  } finally {
    loading.set(false)
  }
}

export function setPageSize(size: number): void {
  pageSize.set(size)
}

export function resetToFirstPage(): void {
  pageIndex.set(1)
}
