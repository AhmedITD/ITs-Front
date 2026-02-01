import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '@/servers/services/AuthService'
import type { User } from '@/types/auth'

const TOKEN_KEY = 'token'
const REFRESH_KEY = 'refreshToken'
const REFRESH_EXPIRES_KEY = 'refreshTokenExpiresAt'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem(TOKEN_KEY))
  const refreshToken = ref<string | null>(localStorage.getItem(REFRESH_KEY))
  const refreshTokenExpiresAt = ref<string | null>(localStorage.getItem(REFRESH_EXPIRES_KEY))
  const user = ref<User | null>(null)

  const isLoggedIn = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.role === 1)

  function setTokens(t: string, r: string, expiresAt: string) {
    token.value = t
    refreshToken.value = r
    refreshTokenExpiresAt.value = expiresAt
    localStorage.setItem(TOKEN_KEY, t)
    localStorage.setItem(REFRESH_KEY, r)
    localStorage.setItem(REFRESH_EXPIRES_KEY, expiresAt)
  }

  function clearAuth() {
    token.value = null
    refreshToken.value = null
    refreshTokenExpiresAt.value = null
    user.value = null
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(REFRESH_KEY)
    localStorage.removeItem(REFRESH_EXPIRES_KEY)
  }

  async function login(email: string, password: string) {
    const res = await authService.login({ email, password })
    if (!res.data?.token) throw new Error(res.message || 'Login failed')
    setTokens(res.data.token, res.data.refreshToken, res.data.refreshTokenExpiresAt)
    await fetchUser()
    return res
  }

  async function register(firstName: string, lastName: string, email: string, password: string) {
    const res = await authService.register({ firstName, lastName, email, password })
    if (!res.data?.token) throw new Error(res.message || 'Registration failed')
    setTokens(res.data.token, res.data.refreshToken, res.data.refreshTokenExpiresAt)
    await fetchUser()
    return res
  }

  async function refresh() {
    if (!refreshToken.value) return
    const res = await authService.refresh(refreshToken.value)
    if (res.data) {
      setTokens(res.data.token, res.data.refreshToken, res.data.refreshTokenExpiresAt)
      return res.data
    }
    throw new Error(res.message || 'Refresh failed')
  }

  async function logout() {
    if (refreshToken.value) {
      try {
        await authService.logout(refreshToken.value)
      } catch {
        // ignore
      }
    }
    clearAuth()
  }

  async function fetchUser() {
    if (!token.value) return
    try {
      const res = await authService.me()
      user.value = res.data ?? null
    } catch {
      user.value = null
    }
  }

  return {
    token,
    refreshToken,
    refreshTokenExpiresAt,
    user,
    isLoggedIn,
    isAdmin,
    login,
    register,
    refresh,
    logout,
    fetchUser,
    clearAuth,
  }
})
