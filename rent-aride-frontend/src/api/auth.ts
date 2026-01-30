import { api, type ApiResponse } from './client'

export interface RegisterRequest {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface User {
  id: number
  firstName: string
  lastName: string
  email: string
  role: number
}

export const authApi = {
  register(data: RegisterRequest) {
    return api.post<ApiResponse<{ id: number; firstName: string; lastName: string; email: string; token: string }>>('/auth/register', data)
  },
  login(data: LoginRequest) {
    return api.post<ApiResponse<{ token: string }>>('/auth/login', data)
  },
  me() {
    return api.get<ApiResponse<User>>('/auth/Me')
  },
}
