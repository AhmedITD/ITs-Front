import { api, type ApiResponse } from './client'

export interface VehicleTypeDto {
  id: number
  name: string
  description?: string
}

export interface VehicleDto {
  id: number
  model: string
  year: number
  licensePlate: string
  dailyPrice: number
  status: string
  vehicleTypeId: number
  vehicleTypeName: string
}

export interface PaginatedList<T> {
  items: T[]
  totalCount: number
  pageIndex: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export const vehiclesApi = {
  getTypes() {
    return api.get<ApiResponse<VehicleTypeDto[]>>('/vehicles/types')
  },
  browse(params: { pageNumber?: number; pageSize?: number; vehicleTypeId?: number }) {
    return api.get<ApiResponse<PaginatedList<VehicleDto>>>('/vehicles', { params })
  },
  create(data: { model: string; year: number; licensePlate: string; dailyPrice: number; vehicleTypeId: number; maintenanceDescription?: string; lastMaintenanceDate?: string; nextMaintenanceDue?: string }) {
    return api.post<ApiResponse<VehicleDto>>('/vehicles', data)
  },
  updatePrice(id: number, dailyPrice: number) {
    return api.put<ApiResponse<VehicleDto>>(`/vehicles/${id}/price`, { dailyPrice })
  },
  delete(id: number) {
    return api.delete<ApiResponse<object>>(`/vehicles/${id}`)
  },
  createType(data: { name: string; description?: string }) {
    return api.post<ApiResponse<VehicleTypeDto>>('/vehicles/types', data)
  },
  updateType(id: number, data: { name: string; description?: string }) {
    return api.put<ApiResponse<VehicleTypeDto>>(`/vehicles/types/${id}`, data)
  },
  deleteType(id: number) {
    return api.delete<ApiResponse<object>>(`/vehicles/types/${id}`)
  },
}
