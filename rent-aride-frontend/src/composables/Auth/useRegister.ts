import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'    
import { useFormErrors } from '@/utils'

const { setFromApi, get, clear, generalError } = useFormErrors()

const router = useRouter()

const firstName = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')
const loading = ref(false)

async function handleSubmit() {
  clear()
  loading.value = true
  try {
    await useAuthStore().register(firstName.value, lastName.value, email.value, password.value)
    await router.push('/')  
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
export { firstName, lastName, email, password, loading }
