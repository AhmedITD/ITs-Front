import { storeToRefs } from 'pinia'
import { useAuthStore } from '@/stores/auth'

export function useAuth() {
  const store = useAuthStore()
  const { user, isLoggedIn, isAdmin } = storeToRefs(store)
  return {
    ...store,
    user,
    isLoggedIn,
    isAdmin,
  }
}
