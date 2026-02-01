import axios, { type AxiosInstance, type AxiosResponse, type AxiosError } from 'axios'
import { API_CONFIG, HTTP_STATUS, ERROR_MESSAGES } from '../config/api'
import type { ApiResponse, ApiException, BackendApiResponse } from '@/types/api'

let isRefreshing = false
let failedQueue: Array<{ resolve: (token: string) => void; reject: (err: unknown) => void }> = []

function processQueue(token: string | null, err: unknown = null) {
  failedQueue.forEach((p) => (err ? p.reject(err) : p.resolve(token!)))
  failedQueue = []
}

function clearAuth() {
  localStorage.removeItem('token')
  localStorage.removeItem('refreshToken')
  localStorage.removeItem('refreshTokenExpiresAt')
}

export class AxiosClient {
  private client: AxiosInstance

  constructor() {
    this.client = axios.create({
      baseURL: API_CONFIG.baseURL,
      timeout: API_CONFIG.timeout,
      headers: API_CONFIG.headers,
    })
    this.setupInterceptors()
  }

  private setupInterceptors() {
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem('token')
      if (token) config.headers.Authorization = `Bearer ${token}`
      return config
    })

    this.client.interceptors.response.use(
      (res) => res,
      async (err: AxiosError) => {
        const originalRequest = err.config as { _retry?: boolean } & NonNullable<AxiosError['config']>

        if (err.response?.status === 401 && originalRequest && !originalRequest._retry) {
          if (isRefreshing) {
            return new Promise((resolve, reject) => {
              failedQueue.push({
                resolve: (token: string) => {
                  if (originalRequest.headers) originalRequest.headers.Authorization = `Bearer ${token}`
                  resolve(this.client(originalRequest))
                },
                reject,
              })
            })
          }

          originalRequest._retry = true
          isRefreshing = true

          try {
            const refreshToken = localStorage.getItem('refreshToken')
            if (!refreshToken) {
              clearAuth()
              window.location.href = '/login'
              return Promise.reject(err)
            }

            const { data } = await axios.post<BackendApiResponse<{ token: string; refreshToken: string; refreshTokenExpiresAt: string }>>(
              `${API_CONFIG.baseURL}/auth/refresh`,
              { refreshToken }
            )
            if (data.success && data.data) {
              localStorage.setItem('token', data.data.token)
              localStorage.setItem('refreshToken', data.data.refreshToken)
              localStorage.setItem('refreshTokenExpiresAt', data.data.refreshTokenExpiresAt)
              if (originalRequest.headers) originalRequest.headers.Authorization = `Bearer ${data.data.token}`
              processQueue(data.data.token)
              return this.client(originalRequest)
            }

            processQueue(null, err)
            clearAuth()
            window.location.href = '/login'
            return Promise.reject(err)
          } catch {
            processQueue(null, err)
            clearAuth()
            window.location.href = '/login'
            return Promise.reject(err)
          } finally {
            isRefreshing = false
          }
        }

        return Promise.reject(err)
      }
    )
  }

  private transformResponse<T>(response: AxiosResponse<BackendApiResponse<T>>): ApiResponse<T> {
    const body = response.data
    const status = response.status
    if (body && typeof body === 'object' && body.success && body.data !== undefined) {
      return {
        data: body.data,
        status,
        message: body.message,
      }
    }
    return {
      data: body?.data as T,
      status,
      message: body?.message,
    }
  }

  private transformError(error: AxiosError<BackendApiResponse<unknown>>): ApiException {
    if (error.response) {
      const body = error.response.data
      return {
        message: body?.message || this.getErrorMessage(error.response.status),
        status: error.response.status,
        errors: body?.errors,
        details: body,
      }
    }
    if (error.code === 'ECONNABORTED') {
      return { message: ERROR_MESSAGES.TIMEOUT_ERROR, status: 408 }
    }
    return {
      message: (error as Error).message || ERROR_MESSAGES.NETWORK_ERROR,
      status: 0,
      details: error,
    }
  }

  private getErrorMessage(status: number): string {
    switch (status) {
      case HTTP_STATUS.UNAUTHORIZED:
        return ERROR_MESSAGES.UNAUTHORIZED
      case HTTP_STATUS.FORBIDDEN:
        return ERROR_MESSAGES.FORBIDDEN
      case HTTP_STATUS.NOT_FOUND:
        return ERROR_MESSAGES.NOT_FOUND
      case HTTP_STATUS.UNPROCESSABLE_ENTITY:
        return ERROR_MESSAGES.VALIDATION_ERROR
      case HTTP_STATUS.INTERNAL_SERVER_ERROR:
        return ERROR_MESSAGES.SERVER_ERROR
      default:
        return ERROR_MESSAGES.UNKNOWN_ERROR
    }
  }

  async get<T>(url: string, config?: { params?: Record<string, unknown> }): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.get<BackendApiResponse<T>>(url, config)
      return this.transformResponse(response)
    } catch (error) {
      throw this.transformError(error as AxiosError<BackendApiResponse<unknown>>)
    }
  }

  async post<T>(url: string, data?: unknown): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.post<BackendApiResponse<T>>(url, data)
      return this.transformResponse(response)
    } catch (error) {
      throw this.transformError(error as AxiosError<BackendApiResponse<unknown>>)
    }
  }

  async put<T>(url: string, data?: unknown): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.put<BackendApiResponse<T>>(url, data)
      return this.transformResponse(response)
    } catch (error) {
      throw this.transformError(error as AxiosError<BackendApiResponse<unknown>>)
    }
  }

  async delete<T>(url: string): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.delete<BackendApiResponse<T>>(url)
      return this.transformResponse(response)
    } catch (error) {
      throw this.transformError(error as AxiosError<BackendApiResponse<unknown>>)
    }
  }

  getAxiosInstance(): AxiosInstance {
    return this.client
  }
}

export const axiosClient = new AxiosClient()
