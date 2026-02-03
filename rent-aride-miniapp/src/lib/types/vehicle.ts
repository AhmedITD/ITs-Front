export interface VehicleTypeDto {
  id: number
  name: string
  description?: string
}

export interface VehicleDto {
  id: number
  model: string
  year: number
  licensePlate: string
  dailyPrice: number
  status: string
  vehicleTypeId: number
  vehicleTypeName: string
}
