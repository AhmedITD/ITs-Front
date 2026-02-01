import { axiosClient } from '../core/AxiosClient'
import { API_ENDPOINTS } from '../config/api'
import type { User, LoginRequest, RegisterRequest, LoginResponse } from '@/types/auth'
import type { ApiResponse } from '@/types/api'

export class AuthService {
  private base = API_ENDPOINTS.auth

  async register(data: RegisterRequest): Promise<ApiResponse<LoginResponse>> {
    const res = await axiosClient.post<LoginResponse>(`${this.base}/register`, data)
    return res
  }

  async login(data: LoginRequest): Promise<ApiResponse<LoginResponse>> {
    const res = await axiosClient.post<LoginResponse>(`${this.base}/login`, data)
    return res
  }

  async refresh(refreshToken: string): Promise<ApiResponse<LoginResponse>> {
    const res = await axiosClient.post<LoginResponse>(`${this.base}/refresh`, { refreshToken })
    return res
  }

  async logout(refreshToken: string): Promise<ApiResponse<object>> {
    const res = await axiosClient.post<object>(`${this.base}/logout`, { refreshToken })
    return res
  }

  async logoutAllDevices(): Promise<ApiResponse<object>> {
    const res = await axiosClient.post<object>(`${this.base}/logoutAllDevices`)
    return res
  }

  async me(): Promise<ApiResponse<User>> {
    const res = await axiosClient.get<User>(`${this.base}/Me`)
    return res
  }
}

export const authService = new AuthService()
