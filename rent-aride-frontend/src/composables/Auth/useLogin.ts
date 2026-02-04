import type { RouteLocationNormalizedLoaded, Router } from 'vue-router'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useFormErrors } from '@/utils'

const { setFromApi, get, clear, generalError } = useFormErrors()

const email = ref('')
const password = ref('')
const loading = ref(false)

/** Call from component setup with useRouter() and useRoute() so router is in injection context. */
function useLogin(router: Router, route: RouteLocationNormalizedLoaded) {
  async function handleSubmit() {
    clear()
    loading.value = true
    try {
      await useAuthStore().login(email.value, password.value)
      const redirect = (route?.query?.redirect as string) || '/'
      await router.push(redirect)
    } catch (e) {
      setFromApi(e)
    } finally {
      loading.value = false
    }
  }
  return { handleSubmit }
}

// functions
export { useLogin }
// stores / form state
export { useAuthStore, get, clear, generalError }
export { email, password, loading }
