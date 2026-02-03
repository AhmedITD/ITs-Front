export interface RentalHistoryItemDto {
  id: number
  startDate: string
  endDate: string
  totalPrice: number
  status: string
  vehicleModel: string
  licensePlate: string
}

export interface InvoiceResponse {
  invoiceId?: string
  paymentUrl: string
}
