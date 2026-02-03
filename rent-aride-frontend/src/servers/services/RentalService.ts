import { axiosClient } from '../core/AxiosClient'
import { API_ENDPOINTS } from '../config/api'
import type { InvoiceResponse, RentalHistoryItemDto } from '@/types/rental'
import type { PaginatedList } from '@/types/common'
import type { ApiResponse } from '@/types/api'

export class RentalService {
  private base = API_ENDPOINTS.rentals

  /** Create an invoice and get Qi payment URL; redirect user to paymentUrl to pay. */
  async createInvoice(data: {
    vehicleId: number
    startDate: string
    endDate: string
    amenityIds: number[]
    finishUrl: string
  }): Promise<ApiResponse<InvoiceResponse>> {
    return axiosClient.post<InvoiceResponse>(`${this.base}/invoice`, data)
  }

  async getMyHistory(params: {
    pageNumber?: number
    pageSize?: number
    status?: string
    startDateFrom?: string
    startDateTo?: string
    minPrice?: number
    maxPrice?: number
    searchTerm?: string
  }): Promise<ApiResponse<PaginatedList<RentalHistoryItemDto>>> {
    const cleaned: Record<string, string | number> = {}
    if (params.pageNumber != null) cleaned.pageNumber = params.pageNumber
    if (params.pageSize != null) cleaned.pageSize = params.pageSize
    if (params.status) cleaned.status = params.status
    if (params.startDateFrom) cleaned.startDateFrom = params.startDateFrom
    if (params.startDateTo) cleaned.startDateTo = params.startDateTo
    if (params.minPrice != null) cleaned.minPrice = params.minPrice
    if (params.maxPrice != null) cleaned.maxPrice = params.maxPrice
    if (params.searchTerm) cleaned.searchTerm = params.searchTerm
    return axiosClient.get<PaginatedList<RentalHistoryItemDto>>(`${this.base}/my-history`, {
      params: cleaned,
    })
  }
}

export const rentalService = new RentalService()
