import { axiosClient } from '../core/AxiosClient'
import { API_ENDPOINTS } from '../config/api'
import type { AmenityDto } from '@/types/amenity'
import type { ApiResponse } from '@/types/api'

export class AmenityService {
  private base = API_ENDPOINTS.amenities

  async getAll(): Promise<ApiResponse<AmenityDto[]>> {
    return axiosClient.get<AmenityDto[]>(this.base)
  }

  async create(data: { name: string; price: number }): Promise<ApiResponse<AmenityDto>> {
    return axiosClient.post<AmenityDto>(this.base, data)
  }

  async update(id: number, data: { name: string; price: number }): Promise<ApiResponse<AmenityDto>> {
    return axiosClient.put<AmenityDto>(`${this.base}/${id}`, data)
  }

  async delete(id: number): Promise<ApiResponse<object>> {
    return axiosClient.delete<object>(`${this.base}/${id}`)
  }
}

export const amenityService = new AmenityService()
