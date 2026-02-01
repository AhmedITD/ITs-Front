export interface ApiResponse<T> {
  data: T
  status: number
  message?: string
}

export interface BackendApiResponse<T> {
  success: boolean
  data?: T
  message?: string
  errors?: Record<string, string[]>
}

export interface ApiException {
  message: string
  status: number
  code?: string
  errors?: Record<string, string[]>
  details?: unknown
}
