/**
 * Encode charge/pay session into QR payload string.
 */

/** @param {object} payload */
export function encodeCharge(payload) {
  const data = {
    type: 'charge',
    sessionId: payload.sessionId,
    amount: Number(payload.amount),
    parkId: payload.parkId || 'default',
    parkName: payload.parkName || 'Parking',
    ts: Date.now(),
  };
  return JSON.stringify(data);
}

/** @param {object} payload - User pay QR (admin scans to charge) */
export function encodeUserPay(payload) {
  const data = {
    type: 'userpay',
    userId: payload.userId,
    userName: payload.userName || 'User',
    ts: Date.now(),
  };
  return JSON.stringify(data);
}
