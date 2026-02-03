/** Re-exports from inline bridge in index.html (window.__RENTARIDE_BRIDGE__). */
const B = typeof window !== 'undefined' ? window.__RENTARIDE_BRIDGE__ : null
const base = () => (typeof window !== 'undefined' && window.__RENTARIDE_API_BASE__) || 'http://localhost:5022'

export const getAuthCode = B?.getAuthCode ?? (() => { throw new Error('Bridge not loaded') })
export const isSuperQi = B?.isSuperQi ?? false
export const authWithSuperQi = B?.authWithSuperQi ?? (() => { throw new Error('Bridge not loaded') })
export const tradePayWithUrl = B?.tradePayWithUrl ?? (() => { throw new Error('Bridge not loaded') })
export const alert = B?.alert ?? ((x) => window.alert(x))
export const allowSystemSnapshot = B?.allowSystemSnapshot ?? (() => {})

export const API_BASE = B?.API_BASE ?? base()
export const ENDPOINTS = B?.ENDPOINTS ?? {
  authWithSuperQi: base() + '/auth/auth-with-superQi',
  vehicles: base() + '/vehicles',
  vehiclesTypes: base() + '/vehicles/types',
  rentals: base() + '/rentals',
  rentalsInvoice: base() + '/rentals/invoice',
  rentalsMyHistory: base() + '/rentals/my-history',
  amenities: base() + '/amenities',
}
