import type { RouteLocationNormalized, NavigationGuardNext } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

export async function requireAuth(
  to: RouteLocationNormalized,
  _from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  const auth = useAuthStore()
  // if (auth.token && !auth.user) await auth.fetchUser()
  if (!auth.isLoggedIn) return next({ name: 'login', query: { redirect: to.fullPath } })
  next()
}

export function requireGuest(_to: RouteLocationNormalized, _from: RouteLocationNormalized, next: NavigationGuardNext) {
  const auth = useAuthStore()
  if (auth.isLoggedIn) return next({ name: 'home' })
  next()
}

export function requireAdmin(_to: RouteLocationNormalized, _from: RouteLocationNormalized, next: NavigationGuardNext) {
  const auth = useAuthStore()
  if (!auth.isAdmin) return next({ name: 'home' })
  next()
}
