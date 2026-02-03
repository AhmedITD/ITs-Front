import { getApi } from './client.js'
import type { AmenityDto } from '../types/amenity.js'

export async function getAll(): Promise<AmenityDto[]> {
  const res = (await getApi('/amenities')) as { data?: AmenityDto[] } | AmenityDto[]
  if (Array.isArray(res)) return res
  return (res as { data?: AmenityDto[] })?.data ?? []
}
