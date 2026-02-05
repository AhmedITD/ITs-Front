import { httpClient } from '../core/HttpClient.js'
import { API_ENDPOINTS } from '../config/api.js'
import type { AmenityDto } from '../../types/amenity.js'
import type { ApiResponse } from '../../types/api.js'

export class AmenityService {
  private base = API_ENDPOINTS.amenities

  async getAll(): Promise<ApiResponse<AmenityDto[]>> {
    return httpClient.get<AmenityDto[]>(this.base)
  }
}

export const amenityService = new AmenityService()
