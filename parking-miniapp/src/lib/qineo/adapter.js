import { ENDPOINTS } from './config.js';

const isSuperQi = typeof my !== 'undefined';
const NOT_SUPERQI = 'Must run inside SuperQi';

function formatSdkError(err, fallback) {
  if (!err) return fallback;
  const code = err.error ?? err.errorCode ?? err.code;
  const msg = err.errMsg ?? err.message ?? err.msg;
  if (code != null && msg) return `${fallback}: [${code}] ${msg}`;
  if (msg) return `${fallback}: ${msg}`;
  if (code != null) return `${fallback}: code ${code}`;
  return fallback;
}

export async function authWithSuperQi(token) {
  try {
    const res = await fetch(ENDPOINTS.auth, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ token }),
    });
    if (!res.ok) {
      const body = await res.text();
      const msg = body ? `Auth failed (${res.status}): ${body}` : `Auth failed (${res.status})`;
      throw new Error(msg);
    }
    return res.json();
  } catch (e) {
    if (e instanceof Error && e.message.startsWith('Auth failed')) throw e;
    throw new Error(`Auth request failed: ${e?.message ?? e}`);
  }
}

export async function getAuthCode(options = {}) {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);
  const code = await new Promise((resolve, reject) => {
    my.getAuthCode({
      scopes: options.scopes || ['auth_base', 'USER_ID'],
      success: (res) => resolve(res.authCode || res.token || res.code || ''),
      fail: (err) => reject(new Error(formatSdkError(err, 'getAuthCode failed'))),
    });
  });
  return { code };
}

export { isSuperQi };

export function allowSystemSnapshot() {
  if (!isSuperQi || !my.allowSystemSnapshot) return;
  my.allowSystemSnapshot({ allow: true });
}

export async function tradePay(params) {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);

  const headers = { 'Content-Type': 'application/json' };
  if (params.token) headers['Authorization'] = params.token;

  let res;
  try {
    res = await fetch(ENDPOINTS.payment, {
      method: 'POST',
      headers,
      body: JSON.stringify({
        orderId: params.orderId,
        amount: params.amount,
        subject: params.subject || 'Parking',
      }),
    });
  } catch (e) {
    throw new Error(`Payment request failed: ${e?.message ?? e}`);
  }
  if (!res.ok) {
    const body = await res.text();
    const msg = body ? `Payment request failed (${res.status}): ${body}` : `Payment request failed (${res.status})`;
    throw new Error(msg);
  }

  const data = await res.json();
  const url = data.url ?? data.paymentUrl;
  if (!url) throw new Error('No payment URL in response');

  return new Promise((resolve, reject) => {
    my.tradePay({
      paymentUrl: url,
      success: (res) => resolve({ success: true, ...res }),
      fail: (err) => reject(new Error(formatSdkError(err, 'Payment failed'))),
    });
  });
}

export async function scan() {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);
  return new Promise((resolve, reject) => {
    (my.scan || my.device?.scan)({
      success: (res) => resolve(res.result || res.data || res.code || ''),
      fail: (err) => reject(new Error(formatSdkError(err, 'Scan failed'))),
    });
  });
}
