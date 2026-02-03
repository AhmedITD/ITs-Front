import { getApi } from './client.js'
import type { VehicleTypeDto, VehicleDto } from '../types/vehicle.js'
import type { PaginatedList } from '../types/common.js'

export async function getTypes(): Promise<VehicleTypeDto[]> {
  const res = (await getApi('/vehicles/types')) as { data?: VehicleTypeDto[] } | VehicleTypeDto[]
  if (Array.isArray(res)) return res
  return (res as { data?: VehicleTypeDto[] })?.data ?? []
}

export interface BrowseParams {
  pageNumber?: number
  pageSize?: number
  vehicleTypeId?: number
  status?: string
  searchTerm?: string
}

export async function browse(params: BrowseParams = {}): Promise<PaginatedList<VehicleDto>> {
  const res = (await getApi('/vehicles', params as Record<string, string | number>)) as
    | { data?: PaginatedList<VehicleDto> }
    | PaginatedList<VehicleDto>
  const data = (res as { data?: PaginatedList<VehicleDto> })?.data ?? res
  return Array.isArray(data)
    ? { items: data, totalCount: 0, totalPages: 0 }
    : {
        items: (data as PaginatedList<VehicleDto>)?.items ?? [],
        totalCount: (data as PaginatedList<VehicleDto>)?.totalCount ?? 0,
        totalPages: (data as PaginatedList<VehicleDto>)?.totalPages ?? 0,
      }
}
