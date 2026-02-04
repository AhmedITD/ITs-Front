/**
 * Vehicles composable: init + store exports (same idea as frontend useVehicles).
 */
import {
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
  loadTypes,
  loadVehicles,
  setPageSize,
  resetToFirstPage,
} from '../../stores/vehicle.js'
import type { VehicleDto } from '../../types/vehicle.js'

export {
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
  loadTypes,
  loadVehicles,
  setPageSize,
  resetToFirstPage,
}

export function initVehicles(initialPageSize = 10): Promise<void> {
  setPageSize(initialPageSize)
  return loadTypes()
}

/** Navigate to book page with vehicle pre-selected (same concept as frontend book(vehicle)). */
export function book(vehicle: VehicleDto): string {
  return `/book?vehicleId=${vehicle.id}`
}
