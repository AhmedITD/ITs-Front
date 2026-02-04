import { getApi } from './client.js'
import type { AmenityDto } from '../types/amenity.js'

export async function getAll(): Promise<AmenityDto[]> {
  const res = (await getApi('/amenities')) as { data?: AmenityDto[] }
  return res.data ?? []
}
