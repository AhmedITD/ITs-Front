/**
 * Parse and validate scanned QR payload.
 */

/**
 * @param {string} raw
 * @returns {{ type: string, sessionId?: string, amount?: number, parkId?: string, parkName?: string, userId?: string, userName?: string, valid: boolean, error?: string }}
 */
export function decode(raw) {
  if (!raw || typeof raw !== 'string') {
    return { valid: false, error: 'Invalid QR content' };
  }
  const trimmed = raw.trim();
  try {
    const data = JSON.parse(trimmed);
    if (data.type === 'charge') {
      const sessionId = data.sessionId;
      const amount = Number(data.amount);
      if (!sessionId || !(amount > 0)) {
        return { valid: false, error: 'Invalid charge QR' };
      }
      return {
        valid: true,
        type: 'charge',
        sessionId,
        amount,
        parkId: data.parkId,
        parkName: data.parkName,
      };
    }
    if (data.type === 'userpay') {
      const userId = data.userId;
      if (!userId) {
        return { valid: false, error: 'Invalid user-pay QR' };
      }
      return {
        valid: true,
        type: 'userpay',
        userId,
        userName: data.userName,
      };
    }
    return { valid: false, error: 'Unknown QR type' };
  } catch (_) {
    return { valid: false, error: 'Invalid QR format' };
  }
}
