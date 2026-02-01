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

  async getMyHistory(pageNumber = 1, pageSize = 10): Promise<ApiResponse<PaginatedList<RentalHistoryItemDto>>> {
    return axiosClient.get<PaginatedList<RentalHistoryItemDto>>(`${this.base}/my-history`, {
      params: { pageNumber, pageSize },
    })
  }
}

export const rentalService = new RentalService()
