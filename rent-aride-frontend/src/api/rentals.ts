import { api, type ApiResponse } from './client'

export interface RentalDto {
  id: number
  startDate: string
  endDate: string
  totalPrice: number
  status: string
  vehicleId: number
  vehicleModel: string
  amenityNames: string[]
}

export interface RentalHistoryItemDto {
  id: number
  startDate: string
  endDate: string
  totalPrice: number
  status: string
  vehicleModel: string
  licensePlate: string
}

export interface PaginatedList<T> {
  items: T[]
  totalCount: number
  pageIndex: number
  totalPages: number
}

export const rentalsApi = {
  create(data: { vehicleId: number; startDate: string; endDate: string; amenityIds: number[] }) {
    return api.post<ApiResponse<RentalDto>>('/rentals', data)
  },
  getMyHistory(pageNumber = 1, pageSize = 10) {
    return api.get<ApiResponse<PaginatedList<RentalHistoryItemDto>>>('/rentals/my-history', { params: { pageNumber, pageSize } })
  },
}
