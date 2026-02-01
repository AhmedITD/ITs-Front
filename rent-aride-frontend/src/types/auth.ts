export interface User {
  id: number
  firstName: string
  lastName: string
  email: string
  role: number
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface LoginResponse {
  token: string
  refreshToken: string
  refreshTokenExpiresAt: string
}
