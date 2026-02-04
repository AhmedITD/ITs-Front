import { getApi } from './client.js'
import type { VehicleTypeDto, VehicleDto } from '../types/vehicle.js'
import type { PaginatedList } from '../types/common.js'

export async function getTypes(): Promise<VehicleTypeDto[]> {
  const res = (await getApi('/vehicles/types')) as { data?: VehicleTypeDto[] }
  return res.data ?? []
}

export interface BrowseParams {
  pageNumber?: number
  pageSize?: number
  vehicleTypeId?: number
  status?: string
  searchTerm?: string
}

export async function browse(params: BrowseParams = {}): Promise<PaginatedList<VehicleDto>> {
  const res = (await getApi('/vehicles', params as ApiParams)) as { data?: PaginatedList<VehicleDto> }
  return res.data ?? { items: [], totalCount: 0, totalPages: 0 }
}
