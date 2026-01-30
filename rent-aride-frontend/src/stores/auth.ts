import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi, type User } from '@/api/auth'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const user = ref<User | null>(null)

  const isLoggedIn = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.role === 1)

  async function login(email: string, password: string) {
    const { data } = await authApi.login({ email, password })
    if (!data.success || !data.data?.token) throw new Error(data.message || 'Login failed')
    token.value = data.data.token
    localStorage.setItem('token', data.data.token)
    await fetchUser()
    return data
  }

  async function register(firstName: string, lastName: string, email: string, password: string) {
    const { data } = await authApi.register({ firstName, lastName, email, password })
    if (!data.success || !data.data?.token) throw new Error(data.message || 'Registration failed')
    token.value = data.data.token
    localStorage.setItem('token', data.data.token)
    await fetchUser()
    return data
  }

  async function fetchUser() {
    if (!token.value) return
    try {
      const { data } = await authApi.me()
      if (data.success && data.data) user.value = data.data
      else user.value = null
    } catch {
      user.value = null
    }
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
  }

  return { token, user, isLoggedIn, isAdmin, login, register, fetchUser, logout }
})
