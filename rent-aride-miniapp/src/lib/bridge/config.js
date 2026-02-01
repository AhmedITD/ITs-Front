/**
 * API configuration for RentARide mini app.
 * Update VITE_API_URL in .env for different environments.
 */

export const API_BASE = import.meta.env.VITE_API_URL || 'http://localhost:5022';

export const ENDPOINTS = {
  authWithSuperQi: `${API_BASE}/auth/auth-with-superQi`,
  vehicles: `${API_BASE}/vehicles`,
  vehiclesTypes: `${API_BASE}/vehicles/types`,
  rentals: `${API_BASE}/rentals`,
  rentalsInvoice: `${API_BASE}/rentals/invoice`,
  rentalsMyHistory: `${API_BASE}/rentals/my-history`,
  amenities: `${API_BASE}/amenities`,
};
