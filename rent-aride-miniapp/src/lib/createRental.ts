/**
 * Create rental flow: state + loadFormData, onSubmit, toggleAmenity (same idea as frontend useCreateRental).
 */
import { writable, get } from 'svelte/store'
import * as vehiclesApi from './api/vehicles.js'
import * as amenitiesApi from './api/amenities.js'
import * as rentalsApi from './api/rentals.js'
import type { VehicleDto } from './types/vehicle.js'
import type { AmenityDto } from './types/amenity.js'

export const vehicles = writable<VehicleDto[]>([])
export const amenities = writable<AmenityDto[]>([])
export const selectedVehicleId = writable<number | null>(null)
export const startDate = writable('')
export const endDate = writable('')
export const selectedAmenityIds = writable<number[]>([])
export const loading = writable(false)
export const loadingForm = writable(true)
export const generalError = writable('')

function toUtcIso(dateOnly: string, endOfDay: boolean): string {
  return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`
}

export async function loadFormData(vehicleIdFromQuery: number | null): Promise<void> {
  loadingForm.set(true)
  generalError.set('')
  try {
    const [vRes, aRes] = await Promise.all([
      vehiclesApi.browse({ pageSize: 500 }),
      amenitiesApi.getAll(),
    ])
    vehicles.set((vRes.items ?? []).filter((v) => v.status === 'Available'))
    amenities.set(Array.isArray(aRes) ? aRes : (aRes ?? []))
    const vehs = get(vehicles)
    if (vehicleIdFromQuery != null && vehs.some((v) => v.id === vehicleIdFromQuery)) {
      selectedVehicleId.set(vehicleIdFromQuery)
    } else if (vehs.length > 0 && get(selectedVehicleId) == null) {
      selectedVehicleId.set(vehs[0]?.id ?? null)
    }
  } catch (e) {
    generalError.set((e as Error)?.message || 'Failed to load form data')
  } finally {
    loadingForm.set(false)
  }
}

export async function onSubmit(
  ensureAuth: () => Promise<void>,
  finishUrl: string,
  onPaymentUrl: (url: string) => void | Promise<void>,
  onError: (msg: string) => void
): Promise<void> {
  const vehicleId = get(selectedVehicleId)
  const start = get(startDate)
  const end = get(endDate)
  if (!vehicleId || !start || !end) {
    generalError.set('Please select a vehicle and dates.')
    return
  }
  generalError.set('')
  loading.set(true)
  try {
    await ensureAuth()
    const res = await rentalsApi.createInvoice({
      vehicleId,
      startDate: toUtcIso(start, false),
      endDate: toUtcIso(end, true),
      amenityIds: get(selectedAmenityIds),
      finishUrl,
    })
    const paymentUrl = res?.paymentUrl ?? (res as { data?: { paymentUrl?: string } })?.data?.paymentUrl
    if (paymentUrl) {
      await onPaymentUrl(paymentUrl)
    } else {
      generalError.set((res as { message?: string })?.message || 'Failed to create payment link')
    }
  } catch (e) {
    const msg = (e as Error)?.message || 'Something went wrong'
    generalError.set(msg)
    onError(msg)
  } finally {
    loading.set(false)
  }
}

export function toggleAmenity(id: number): void {
  selectedAmenityIds.update((ids) => {
    const i = ids.indexOf(id)
    if (i === -1) return [...ids, id]
    return ids.filter((x) => x !== id)
  })
}
