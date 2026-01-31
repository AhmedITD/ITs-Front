/**
 * Qineo platform + backend API layer.
 * Single entry point for auth, payment, and scan.
 */

export { getAuthCode, isSuperQi, tradePay, scan, authWithSuperQi } from './adapter.js';
