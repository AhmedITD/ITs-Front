/**
 * Auth store: SuperQi login (getAuthCode → backend exchange), authError for getAuthCode/backend failures.
 * Doc: https://superqi-dev-docs.pages.dev/api-reference/flows/authentication
 */
import { writable } from 'svelte/store'
// import { getAuthCode, authWithSuperQi } from '../bridge/index.js'

const AUTH_KEY = 'rentaride_miniapp_auth'

export interface AuthUserInfo {
  id?: string | number
  name?: string
  [key: string]: unknown
}

export interface AuthData {
  code: string
  token: string
  refreshToken?: string | null
  refreshTokenExpiresAt?: string | null
  userInfo: AuthUserInfo
}

function loadStored(): AuthData | null {
  try {
    const s = localStorage.getItem(AUTH_KEY)
    if (s) return JSON.parse(s) as AuthData
  } catch {
    // ignore
  }
  return null
}

export const auth = writable<AuthData | null>(loadStored())

/** Last login error (e.g. getAuthCode fail, user denied, or backend "Invalid or expired SuperQi auth code"). Cleared on successful login or clearAuthError(). */
export const authError = writable<string | null>(null)

auth.subscribe((v) => {
  if (v) localStorage.setItem(AUTH_KEY, JSON.stringify(v))
  else localStorage.removeItem(AUTH_KEY)
})

export function clearAuthError(): void {
  authError.set(null)
}

export async function login(): Promise<AuthData> {
  clearAuthError()
  let res: { code: string }
  try {
    res = await getAuthCode({ scopes: ['auth_base', 'USER_ID'] })
  } catch (err) {
    const msg = err instanceof Error ? err.message : String(err)
    authError.set(msg)
    throw err
  }

  let token: string | null = null
  let refreshToken: string | null = null
  let refreshTokenExpiresAt: string | null = null
  let userInfo: AuthUserInfo = {}

  if (res.code) {
    try {
      const data = await authWithSuperQi(res.code)
      token = (data as { token?: string })?.token ?? null
      refreshToken = (data as { refreshToken?: string })?.refreshToken ?? null
      refreshTokenExpiresAt =
        (data as { refreshTokenExpiresAt?: string })?.refreshTokenExpiresAt ?? null
      userInfo =
        (data as { userInfo?: AuthUserInfo })?.userInfo ??
        (data as { user?: AuthUserInfo })?.user ??
        ((data as { id?: string }).id
          ? { id: (data as { id: string }).id, name: (data as { name?: string }).name ?? 'User' }
          : {})
    } catch (err) {
      const msg = err instanceof Error ? err.message : String(err)
      authError.set(msg)
      throw err
    }
  }

  if (!userInfo || Object.keys(userInfo).length === 0) {
    userInfo = { id: res.code?.slice(-8), name: 'User' }
  }
  const authData: AuthData = {
    code: res.code,
    token: token ?? res.code,
    refreshToken,
    refreshTokenExpiresAt,
    userInfo,
  }
  auth.set(authData)
  return authData
}

export function logout(): void {
  auth.set(null)
  clearAuthError()
}
