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
  searchQuery,
  loading,
  loadingTypes,
  loadTypes,
  loadVehicles,
  setPageSize,
  resetToFirstPage,
} from './stores/vehicle.js'

export {
  types,
  vehicles,
  totalPages,
  totalCount,
  pageIndex,
  pageSize,
  vehicleTypeId,
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
