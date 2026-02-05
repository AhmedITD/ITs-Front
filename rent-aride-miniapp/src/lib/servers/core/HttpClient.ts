import { get } from 'svelte/store'
import { auth } from '../../stores/auth.js'
import { API_CONFIG, HTTP_STATUS, ERROR_MESSAGES } from '../config/api.js'
import type { ApiResponse, BackendApiResponse, ApiException } from '../../types/api.js'

function buildUrl(path: string, params: Record<string, string | number> = {}): string {
  const base = API_CONFIG.baseURL
  const p = path.startsWith('/') ? path : '/' + path
  const url = new URL(p, base)
  Object.entries(params).forEach(([k, v]) => {
    if (v != null && v !== '') url.searchParams.set(k, String(v))
  })
  return url.toString()
}

function transformResponse<T>(data: BackendApiResponse<T> | null, status: number): ApiResponse<T> {
  return {
    data: data?.data as T,
    status,
    message: data?.message ?? '',
  }
}

function transformError(status: number, body: BackendApiResponse<unknown> | null): ApiException {
  return {
    message: body?.message ?? getErrorMessage(status),
    status,
    errors: body?.errors,
    details: body,
  }
}

function getErrorMessage(status: number): string {
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

export class HttpClient {
  async get<T>(
    path: string,
    config?: { params?: Record<string, string | number> }
  ): Promise<ApiResponse<T>> {
    const url = buildUrl(path, config?.params ?? {})
    const headers: Record<string, string> = { ...API_CONFIG.headers }
    const a = get(auth)
    if (a?.token) headers['Authorization'] = `Bearer ${a.token}`

    const res = await fetch(url, { method: 'GET', headers })
    const data = (await res.json().catch(() => null)) as BackendApiResponse<T> | null

    if (!res.ok) {
      throw transformError(res.status, data)
    }
    return transformResponse(data, res.status)
  }

  async post<T>(path: string, body?: unknown): Promise<ApiResponse<T>> {
    const url = buildUrl(path)
    const headers: Record<string, string> = { ...API_CONFIG.headers }
    const a = get(auth)
    if (a?.token) headers['Authorization'] = `Bearer ${a.token}`

    const res = await fetch(url, {
      method: 'POST',
      headers,
      body: body != null ? JSON.stringify(body) : undefined,
    })
    const data = (await res.json().catch(() => null)) as BackendApiResponse<T> | null

    if (!res.ok) {
      throw transformError(res.status, data)
    }
    return transformResponse(data, res.status)
  }

  async put<T>(path: string, body?: unknown): Promise<ApiResponse<T>> {
    const url = buildUrl(path)
    const headers: Record<string, string> = { ...API_CONFIG.headers }
    const a = get(auth)
    if (a?.token) headers['Authorization'] = `Bearer ${a.token}`

    const res = await fetch(url, {
      method: 'PUT',
      headers,
      body: body != null ? JSON.stringify(body) : undefined,
    })
    const data = (await res.json().catch(() => null)) as BackendApiResponse<T> | null

    if (!res.ok) {
      throw transformError(res.status, data)
    }
    return transformResponse(data, res.status)
  }

  async delete<T>(path: string): Promise<ApiResponse<T>> {
    const url = buildUrl(path)
    const headers: Record<string, string> = { ...API_CONFIG.headers }
    const a = get(auth)
    if (a?.token) headers['Authorization'] = `Bearer ${a.token}`

    const res = await fetch(url, { method: 'DELETE', headers })
    const data = (await res.json().catch(() => null)) as BackendApiResponse<T> | null

    if (!res.ok) {
      throw transformError(res.status, data)
    }
    return transformResponse(data, res.status)
  }
}

export const httpClient = new HttpClient()
