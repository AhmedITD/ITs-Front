import { get } from 'svelte/store'
import { auth } from '../stores/auth.js'

function buildUrl(path: string): string {
  const base = import.meta.env.VITE_API_URL
  const p = path.startsWith('/') ? path : '/' + path
  return base + p
}

export interface ApiParams {
  [key: string]: string
}

async function request(
  method: string,
  path: string,
  body: unknown = null,
  params: ApiParams = {}
): Promise<{ data?: unknown; success?: boolean; message?: string; errors?: Record<string, string[]> }> {
  const url = new URL(buildUrl(path))
  Object.entries(params).forEach(([k, v]) => {
    if (v != null && v !== '') url.searchParams.set(k, String(v))
  })

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    Accept: 'application/json',
    'X-Pinggy-No-Screen': '1',
  }
  const a = get(auth)
  if (a?.token) {
    headers['Authorization'] = `Bearer ${a.token}`
  }

  const config: RequestInit = { method, headers }
  if (body != null && (method === 'POST' || method === 'PUT')) {
    config.body = JSON.stringify(body)
  }

  const res = await fetch(url.toString(), config)
  const data = (await res.json().catch(() => ({}))) as {
    data?: unknown
    success?: boolean
    message?: string
    errors?: Record<string, string[]>
  }

  if (!res.ok) {
    const msg =
      data?.message ||
      (data?.errors ? JSON.stringify(data.errors) : null) ||
      `Request failed (${res.status})`
    throw new Error(msg)
  }

  if (data.success === false && data.message) {
    throw new Error(data.message)
  }

  return data
}

export async function getApi(path: string, params: ApiParams = {}): Promise<unknown> {
  return request('GET', path, null, params)
}

export async function postApi(path: string, body: unknown): Promise<unknown> {
  return request('POST', path, body)
}
