/**
 * Create rental flow (same idea as frontend useCreateRental).
 * State + loadFormData, initCreateRental, onSubmit, toggleAmenity, form errors.
 */
import { writable, get } from 'svelte/store'
import { useFormErrors } from '../utils/useFormErrors.js'
import { vehicleService } from '../servers/services/VehicleService.js'
import { amenityService } from '../servers/services/AmenityService.js'
import { rentalService } from '../servers/services/RentalService.js'
import { superQiMiniAppService } from '../servers/services/SuperQiMiniAppService.js'
import type { VehicleDto } from '../types/vehicle.js'
import type { AmenityDto } from '../types/amenity.js'

const { setFromApi, get: getError, clear, generalError } = useFormErrors()

export const vehicles = writable<VehicleDto[]>([])
export const amenities = writable<AmenityDto[]>([])
export const selectedVehicleId = writable<number | null>(null)
export const startDate = writable('')
export const endDate = writable('')
export const selectedAmenityIds = writable<number[]>([])
export const loading = writable(false)
export const loadingForm = writable(true)

export { generalError }

function toUtcIso(dateOnly: string, endOfDay: boolean): string {
  return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`
}

export async function loadFormData(vehicleIdFromQuery: number | null): Promise<void> {
  loadingForm.set(true)
  clear()
  try {
    const [vRes, aRes] = await Promise.all([
      vehicleService.browse({ pageSize: 500 }),
      amenityService.getAll(),
    ])
    if (vRes.data) vehicles.set(vRes.data.items.filter((v) => v.status === 'Available'))
    if (aRes.data) amenities.set(aRes.data)
    const vehs = get(vehicles)
    if (vehicleIdFromQuery != null && vehs.some((v) => v.id === vehicleIdFromQuery)) {
      selectedVehicleId.set(vehicleIdFromQuery)
    } else if (vehs.length > 0 && get(selectedVehicleId) == null) {
      selectedVehicleId.set(vehs[0]?.id ?? null)
    }
  } catch (e) {
    setFromApi(e)
  } finally {
    loadingForm.set(false)
  }
}

export function initCreateRental(vehicleIdFromQuery: number | null): Promise<void> {
  if (vehicleIdFromQuery != null) selectedVehicleId.set(vehicleIdFromQuery)
  return loadFormData(vehicleIdFromQuery)
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
    clear()
    generalError.set('Please select a vehicle and dates.')
    return
  }
  clear()
  loading.set(true)
  try {
    await ensureAuth()
    const res = await rentalService.createInvoice({
      vehicleId,
      startDate: toUtcIso(start, false),
      endDate: toUtcIso(end, true),
      amenityIds: get(selectedAmenityIds),
      finishUrl,
    })
    let paymentUrl: string | undefined = res.data?.paymentUrl
    let superQiErrorSet = false
    const invoiceId = res.data?.invoiceId
    if (invoiceId) {
      try {
        const superQiRes = await superQiMiniAppService.createPaymentForInvoice(
          String(invoiceId),
          finishUrl
        )
        if (superQiRes.data?.paymentUrl) paymentUrl = superQiRes.data.paymentUrl
        else {
          generalError.set(superQiRes.data?.error || superQiRes.message || 'SuperQi payment failed')
          superQiErrorSet = true
        }
      } catch {
        if (!paymentUrl) {
          generalError.set('Could not start SuperQi payment.')
          superQiErrorSet = true
        }
      }
    }
    if (paymentUrl) {
      await onPaymentUrl(paymentUrl)
    } else {
      if (!superQiErrorSet) generalError.set(res.message || 'Failed to create payment link')
      loading.set(false)
    }
  } catch (e) {
    setFromApi(e)
    loading.set(false)
    onError((e as Error)?.message || 'Something went wrong')
  }
}

export function toggleAmenity(id: number): void {
  selectedAmenityIds.update((ids) => {
    const i = ids.indexOf(id)
    if (i === -1) return [...ids, id]
    return ids.filter((x) => x !== id)
  })
}

export { toUtcIso, clear }
export const get = getError
