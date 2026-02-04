/** Re-exports from inline bridge in index.html (window.__RENTARIDE_BRIDGE__). */
const B = typeof window !== 'undefined' ? window.__RENTARIDE_BRIDGE__ : null
const base = () => (typeof window !== 'undefined' && window.__RENTARIDE_API_BASE__) || 'http://localhost:5022'

export const getAuthCode = B?.getAuthCode ?? (() => { throw new Error('Bridge not loaded') })
export const isSuperQi = B?.isSuperQi ?? false
export const authWithSuperQi = B?.authWithSuperQi ?? (() => { throw new Error('Bridge not loaded') })
export const tradePayWithUrl = B?.tradePayWithUrl ?? (() => { throw new Error('Bridge not loaded') })
export const alert = B?.alert ?? ((x) => (typeof window !== 'undefined' ? window.alert(x) : null))
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
  // SuperQi Alipay+ endpoints
  superqiPaymentCreate: base() + '/api/superqi/payment/create',
  superqiPaymentRefund: base() + '/api/superqi/payment/refund',
  superqiNotificationInbox: base() + '/api/superqi/notification/inbox',
  superqiNotificationPush: base() + '/api/superqi/notification/push',
  superqiAgreementPrepare: base() + '/api/superqi/agreement/prepare',
  superqiAgreementApplyToken: base() + '/api/superqi/agreement/apply-token',
  superqiAgreementPay: base() + '/api/superqi/agreement/pay',
}
