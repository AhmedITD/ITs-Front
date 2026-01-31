/**
 * API configuration for parking mini app backend.
 * Update VITE_API_URL in .env for different environments.
 */

export const API_BASE = import.meta.env.VITE_API_URL || 'https://its.mouamle.space';
export const CLIENT_ID = '2025062409117058848604';
export const MERCHANT_ID = '216620000010066056627';

export const ENDPOINTS = {
  auth: `${API_BASE}/api/auth-with-superQi`,
  payment: `${API_BASE}/api/payment`,
};
