import { writable, get } from 'svelte/store'
import * as vehiclesApi from '../api/vehicles.js'
import type { VehicleDto, VehicleTypeDto } from '../types/vehicle.js'

export const types = writable<VehicleTypeDto[]>([])
export const vehicles = writable<VehicleDto[]>([])
export const totalPages = writable(0)
export const totalCount = writable(0)
export const pageIndex = writable(1)
export const pageSize = writable(10)
/** Bound to select: '' for "All", number for type id. */
export const vehicleTypeId = writable<number | string | undefined>(undefined)
export const searchQuery = writable('')
export const loading = writable(false)
export const loadingTypes = writable(true)

export async function loadTypes(): Promise<void> {
  loadingTypes.set(true)
  try {
    const data = await vehiclesApi.getTypes()
    types.set(data)
  } catch {
    types.set([])
  } finally {
    loadingTypes.set(false)
  }
}

export async function loadVehicles(): Promise<void> {
  loading.set(true)
  try {
    const res = await vehiclesApi.browse({
      pageNumber: get(pageIndex),
      pageSize: get(pageSize),
      vehicleTypeId: (() => {
      const v = get(vehicleTypeId)
      return v === '' || v == null ? undefined : Number(v)
    })(),
      searchTerm: get(searchQuery)?.trim() || undefined,
    })
    vehicles.set(res.items)
    totalPages.set(res.totalPages)
    totalCount.set(res.totalCount)
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
