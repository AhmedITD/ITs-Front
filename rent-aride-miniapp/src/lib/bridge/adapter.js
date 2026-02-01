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
    const res = await fetch(ENDPOINTS.authWithSuperQi, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ token }),
    });
    const body = await res.json();
    if (!res.ok) {
      const msg = body?.message || body?.errors ? JSON.stringify(body) : `Auth failed (${res.status})`;
      throw new Error(msg);
    }
    if (body.success && body.data) {
      return body.data;
    }
    return body;
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

/**
 * Call my.tradePay with a payment URL (e.g. from RentARide invoice).
 */
export async function tradePayWithUrl(paymentUrl) {
  if (!isSuperQi) throw new Error(NOT_SUPERQI);
  if (!paymentUrl) throw new Error('Payment URL is required');

  return new Promise((resolve, reject) => {
    my.tradePay({
      paymentUrl,
      success: (res) => resolve({ success: true, ...res }),
      fail: (err) => reject(new Error(formatSdkError(err, 'Payment failed'))),
    });
  });
}

export function alert(content) {
  if (isSuperQi && my.alert) {
    my.alert({ content: String(content) });
  } else {
    window.alert(content);
  }
}
