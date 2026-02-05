import { writable, get } from 'svelte/store'

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
  const fieldErrors = writable<Record<string, string[]>>({})
  const generalError = writable('')

  function setFromApi(err: unknown) {
    const e = err as ApiErrorLike
    fieldErrors.set(e.errors ?? {})
    generalError.set(e.message ?? '')
  }

  function getError(fieldKey: string): string {
    return findFieldError(get(fieldErrors), fieldKey)
  }

  function clear() {
    fieldErrors.set({})
    generalError.set('')
  }

  return { fieldErrors, generalError, setFromApi, get: getError, clear }
}
