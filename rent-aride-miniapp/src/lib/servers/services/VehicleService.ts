import { httpClient } from '../core/HttpClient.js'
import { API_ENDPOINTS } from '../config/api.js'
import type { VehicleDto, VehicleTypeDto } from '../../types/vehicle.js'
import type { PaginatedList } from '../../types/common.js'
import type { ApiResponse } from '../../types/api.js'

export class VehicleService {
  private base = API_ENDPOINTS.vehicles

  async getTypes(): Promise<ApiResponse<VehicleTypeDto[]>> {
    return httpClient.get<VehicleTypeDto[]>(`${this.base}/types`)
  }

  async browse(params: {
    pageNumber?: number
    pageSize?: number
    vehicleTypeId?: number
    status?: string
    searchTerm?: string
  }): Promise<ApiResponse<PaginatedList<VehicleDto>>> {
    const cleaned: Record<string, string | number> = {}
    if (params.pageNumber != null) cleaned.pageNumber = params.pageNumber
    if (params.pageSize != null) cleaned.pageSize = params.pageSize
    if (params.vehicleTypeId != null) cleaned.vehicleTypeId = params.vehicleTypeId
    if (params.status) cleaned.status = params.status
    if (params.searchTerm) cleaned.searchTerm = params.searchTerm
    return httpClient.get<PaginatedList<VehicleDto>>(this.base, { params: cleaned })
  }
}

export const vehicleService = new VehicleService()
