import { api, type ApiResponse } from './client'

export interface AmenityDto {
  id: number
  name: string
  price: number
}

export const amenitiesApi = {
  getAll() {
    return api.get<ApiResponse<AmenityDto[]>>('/amenities')
  },
  create(data: { name: string; price: number }) {
    return api.post<ApiResponse<AmenityDto>>('/amenities', data)
  },
  update(id: number, data: { name: string; price: number }) {
    return api.put<ApiResponse<AmenityDto>>(`/amenities/${id}`, data)
  },
  delete(id: number) {
    return api.delete<ApiResponse<object>>(`/amenities/${id}`)
  },
}
