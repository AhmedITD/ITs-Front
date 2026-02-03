import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useFormErrors } from '@/utils'

const { setFromApi, get, clear, generalError } = useFormErrors()

const route = useRoute()
const router = useRouter()

const email = ref('')
const password = ref('')
const loading = ref(false)

async function handleSubmit() {

  clear()
  loading.value = true
  try {
    await useAuthStore().login(email.value, password.value)
    const redirect = (route?.query.redirect as string) || '/'
    await router.push(redirect)
  } catch (e) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}

// functions
export { handleSubmit }
// stores / form state
export { useAuthStore, get, clear, generalError }
export { email, password, loading }
