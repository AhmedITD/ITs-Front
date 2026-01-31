/**
 * Qineo/SuperQi Mini App API adapter.
 * Requires SuperQi runtime. Throws when not running inside SuperQi.
 */

import { ENDPOINTS } from './config.js';

const isSuperQi = typeof my !== 'undefined';
const NOT_SUPERQI = 'Must run inside SuperQi';

// --- Auth ---

/** Auth with SuperQi token. POST { token } to auth endpoint. */
export async function authWithSuperQi(token) {
  const res = await fetch(ENDPOINTS.auth, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ token }),
  });
  if (!res.ok) throw new Error('Auth failed');
  return res.json();
}

/** Get auth code/token from SuperQi. Returns { code, userInfo? }. */
export async function getAuthCode(_options = {}) {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);
  const code = await new Promise((resolve, reject) => {
    my.getAuthCode({
      success: (res) => resolve(res.authCode || res.token || res.code || ''),
      fail: (err) => reject(err),
    });
  });
  return { code };
}

export { isSuperQi };

// --- Payment ---

/**
 * Initiate payment via backend + my.tradePay.
 * Params: { orderId, amount, subject, token }
 */
export async function tradePay(params) {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);

  const headers = { 'Content-Type': 'application/json' };
  if (params.token) headers['Authorization'] = params.token;

  const res = await fetch(ENDPOINTS.payment, {
    method: 'POST',
    headers,
    body: JSON.stringify({
      orderId: params.orderId,
      amount: params.amount,
      subject: params.subject || 'Parking',
    }),
  });
  if (!res.ok) throw new Error('Payment request failed');

  const { url } = await res.json();
  if (!url) throw new Error('No payment URL');

  return new Promise((resolve, reject) => {
    my.tradePay({
      paymentUrl: url,
      success: (res) => resolve({ success: true, ...res }),
      fail: (err) => reject(err || new Error('Payment failed')),
    });
  });
}

// --- Scan ---

/** Scan QR code. Returns raw string payload. */
export async function scan() {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);
  return new Promise((resolve, reject) => {
    (my.scan || my.device?.scan)({
      success: (res) => resolve(res.result || res.data || res.code || ''),
      fail: (err) => reject(err),
    });
  });
}
