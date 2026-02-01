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

/** Response from POST /rentals/invoice; user is redirected to paymentUrl to pay. */
export interface InvoiceResponse {
  invoiceId: string
  paymentUrl: string
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
