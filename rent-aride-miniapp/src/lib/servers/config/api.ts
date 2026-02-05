export const API_CONFIG = {
  baseURL: import.meta.env.VITE_API_URL ?? '',
  timeout: 90000,
  headers: {
    'Content-Type': 'application/json',
    Accept: 'application/json',
    'X-Pinggy-No-Screen': '1',
  },
} as const

export const API_ENDPOINTS = {
  auth: '/auth',
  vehicles: '/vehicles',
  rentals: '/rentals',
  amenities: '/amenities',
  /** SuperQi MiniApp: payment, refund, notification, agreement. */
  superqi: '/api/superqi',
} as const

export const HTTP_STATUS = {
  OK: 200,
  CREATED: 201,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  UNPROCESSABLE_ENTITY: 422,
  INTERNAL_SERVER_ERROR: 500,
} as const

export const ERROR_MESSAGES = {
  NETWORK_ERROR: 'Network error. Please check your connection.',
  TIMEOUT_ERROR: 'Request timed out. Please try again.',
  UNAUTHORIZED: 'You are not authorized to perform this action.',
  FORBIDDEN: 'Access denied.',
  NOT_FOUND: 'The requested resource was not found.',
  VALIDATION_ERROR: 'Please check your input and try again.',
  SERVER_ERROR: 'Server error. Please try again later.',
  UNKNOWN_ERROR: 'An unexpected error occurred.',
} as const
