import { axiosClient } from '../core/AxiosClient'
import { API_ENDPOINTS } from '../config/api'
import type { VehicleDto, VehicleTypeDto } from '@/types/vehicle'
import type { PaginatedList } from '@/types/common'
import type { ApiResponse } from '@/types/api'

export class VehicleService {
  private base = API_ENDPOINTS.vehicles

  async getTypes(): Promise<ApiResponse<VehicleTypeDto[]>> {
    return axiosClient.get<VehicleTypeDto[]>(`${this.base}/types`)
  }

  async browse(params: { pageNumber?: number; pageSize?: number; vehicleTypeId?: number; status?: string; searchTerm?: string }): Promise<ApiResponse<PaginatedList<VehicleDto>>> {
    return axiosClient.get<PaginatedList<VehicleDto>>(this.base, { params })
  }

  async create(data: {
    model: string
    year: number
    licensePlate: string
    dailyPrice: number
    vehicleTypeId: number
    maintenanceDescription?: string
    lastMaintenanceDate?: string
    nextMaintenanceDue?: string
  }): Promise<ApiResponse<VehicleDto>> {
    return axiosClient.post<VehicleDto>(this.base, data)
  }

  async updatePrice(id: number, dailyPrice: number): Promise<ApiResponse<VehicleDto>> {
    return axiosClient.put<VehicleDto>(`${this.base}/${id}/price`, { dailyPrice })
  }

  async delete(id: number): Promise<ApiResponse<object>> {
    return axiosClient.delete<object>(`${this.base}/${id}`)
  }

  async createType(data: { name: string; description?: string }): Promise<ApiResponse<VehicleTypeDto>> {
    return axiosClient.post<VehicleTypeDto>(`${this.base}/types`, data)
  }

  async updateType(id: number, data: { name: string; description?: string }): Promise<ApiResponse<VehicleTypeDto>> {
    return axiosClient.put<VehicleTypeDto>(`${this.base}/types/${id}`, data)
  }

  async deleteType(id: number): Promise<ApiResponse<object>> {
    return axiosClient.delete<object>(`${this.base}/types/${id}`)
  }
}

export const vehicleService = new VehicleService()
