import { getApi, postApi } from './client.js'
import type { RentalHistoryItemDto, InvoiceResponse } from '../types/rental.js'
import type { PaginatedList } from '../types/common.js'

export interface CreateInvoiceParams {
  vehicleId: number
  startDate: string
  endDate: string
  amenityIds: number[]
  finishUrl: string
}

export async function createInvoice(data: CreateInvoiceParams): Promise<InvoiceResponse> {
  const res = (await postApi('/rentals/invoice', data)) as { data?: InvoiceResponse } & InvoiceResponse
  return res?.data ?? res
}

export interface GetMyHistoryParams {
  pageNumber?: number
  pageSize?: number
  status?: string
  startDateFrom?: string
  startDateTo?: string
  minPrice?: number
  maxPrice?: number
  searchTerm?: string
}

export async function getMyHistory(
  params: GetMyHistoryParams = {}
): Promise<PaginatedList<RentalHistoryItemDto>> {
  const q: Record<string, string | number> = {}
  if (params.pageNumber != null) q.pageNumber = params.pageNumber
  if (params.pageSize != null) q.pageSize = params.pageSize
  if (params.status) q.status = params.status
  if (params.startDateFrom) q.startDateFrom = params.startDateFrom
  if (params.startDateTo) q.startDateTo = params.startDateTo
  if (params.minPrice != null) q.minPrice = params.minPrice
  if (params.maxPrice != null) q.maxPrice = params.maxPrice
  if (params.searchTerm) q.searchTerm = params.searchTerm

  const res = (await getApi('/rentals/my-history', q)) as
    | { data?: PaginatedList<RentalHistoryItemDto> }
    | PaginatedList<RentalHistoryItemDto>
  const data = (res as { data?: PaginatedList<RentalHistoryItemDto> })?.data ?? res
  return {
    items: (data as PaginatedList<RentalHistoryItemDto>)?.items ?? [],
    totalCount: (data as PaginatedList<RentalHistoryItemDto>)?.totalCount ?? 0,
    totalPages: (data as PaginatedList<RentalHistoryItemDto>)?.totalPages ?? 0,
  }
}
