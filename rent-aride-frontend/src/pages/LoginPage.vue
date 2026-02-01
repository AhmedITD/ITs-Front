<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useFormErrors } from '@/composables/useFormErrors'

const router = useRouter()
const route = useRoute()
const auth = useAuthStore()
const { setFromApi, get, clear, generalError } = useFormErrors()

const email = ref('')
const password = ref('')
const loading = ref(false)

async function onSubmit() {
  clear()
  loading.value = true
  try {
    await auth.login(email.value, password.value)
    const redirect = (route.query.redirect as string) || '/'
    await router.push(redirect)
  } catch (e) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="mx-auto max-w-sm py-8">
    <h1 class="mb-6 text-xl font-semibold">Login</h1>
    <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
      <div v-if="generalError && !get('email') && !get('password')" class="rounded-md bg-red-50 p-3 text-sm text-red-700">
        {{ generalError }}
      </div>
      <div>
        <label for="email" class="mb-1 block text-sm font-medium">Email</label>
        <input
          id="email"
          v-model="email"
          type="email"
          required
          autocomplete="email"
          class="w-full rounded-md border px-3 py-2"
          :class="get('email') ? 'border-red-500' : 'border-gray-300'"
        />
        <p v-if="get('email')" class="mt-1 text-sm text-red-600">{{ get('email') }}</p>
      </div>
      <div>
        <label for="password" class="mb-1 block text-sm font-medium">Password</label>
        <input
          id="password"
          v-model="password"
          type="password"
          required
          autocomplete="current-password"
          class="w-full rounded-md border px-3 py-2"
          :class="get('password') ? 'border-red-500' : 'border-gray-300'"
        />
        <p v-if="get('password')" class="mt-1 text-sm text-red-600">{{ get('password') }}</p>
      </div>
      <button
        type="submit"
        class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-60"
        :disabled="loading"
      >
        {{ loading ? 'Signing in…' : 'Sign in' }}
      </button>
    </form>
    <p class="mt-6 text-sm text-gray-600">
      Don't have an account? <router-link to="/register" class="text-indigo-600 hover:underline">Register</router-link>
    </p>
  </div>
</template>
