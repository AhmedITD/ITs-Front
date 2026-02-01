import { ref } from 'vue'

export interface ApiErrorLike {
  message?: string
  errors?: Record<string, string[]>
}

/**
 * Get first error message for a field from API errors.
 * API typically returns PascalCase keys (Email, Password, FirstName).
 */
function findFieldError(errors: Record<string, string[]>, fieldKey: string): string {
  if (!errors || !Object.keys(errors).length) return ''
  const pascal = fieldKey.charAt(0).toUpperCase() + fieldKey.slice(1)
  const camel = fieldKey.charAt(0).toLowerCase() + fieldKey.slice(1)
  const arr = errors[fieldKey] ?? errors[pascal] ?? errors[camel]
  return arr?.length ? (arr[0] ?? '') : ''
}

export function useFormErrors() {
  const fieldErrors = ref<Record<string, string[]>>({})
  const generalError = ref('')

  function setFromApi(err: unknown) {
    const e = err as ApiErrorLike
    fieldErrors.value = e.errors ?? {}
    generalError.value = e.message ?? ''
  }

  function get(fieldKey: string): string {
    return findFieldError(fieldErrors.value, fieldKey)
  }

  function clear() {
    fieldErrors.value = {}
    generalError.value = ''
  }

  return { fieldErrors, generalError, setFromApi, get, clear }
}
