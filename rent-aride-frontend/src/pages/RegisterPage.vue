<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useFormErrors } from '@/composables/useFormErrors'

const router = useRouter()
const auth = useAuthStore()
const { setFromApi, get, clear, generalError } = useFormErrors()

const firstName = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')
const loading = ref(false)

async function onSubmit() {
  clear()
  loading.value = true
  try {
    await auth.register(firstName.value, lastName.value, email.value, password.value)
    await router.push('/')
  } catch (e) {
    setFromApi(e)
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="mx-auto max-w-sm py-8">
    <h1 class="mb-6 text-xl font-semibold">Register</h1>
    <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
      <div
        v-if="generalError && !get('firstName') && !get('lastName') && !get('email') && !get('password')"
        class="rounded-md bg-red-50 p-3 text-sm text-red-700"
      >
        {{ generalError }}
      </div>
      <div class="grid grid-cols-2 gap-4">
        <div>
          <label for="firstName" class="mb-1 block text-sm font-medium">First name</label>
          <input
            id="firstName"
            v-model="firstName"
            type="text"
            required
            autocomplete="given-name"
            class="w-full rounded-md border px-3 py-2"
            :class="get('firstName') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('firstName')" class="mt-1 text-sm text-red-600">{{ get('firstName') }}</p>
        </div>
        <div>
          <label for="lastName" class="mb-1 block text-sm font-medium">Last name</label>
          <input
            id="lastName"
            v-model="lastName"
            type="text"
            required
            autocomplete="family-name"
            class="w-full rounded-md border px-3 py-2"
            :class="get('lastName') ? 'border-red-500' : 'border-gray-300'"
          />
          <p v-if="get('lastName')" class="mt-1 text-sm text-red-600">{{ get('lastName') }}</p>
        </div>
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
          autocomplete="new-password"
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
        {{ loading ? 'Creating account…' : 'Create account' }}
      </button>
    </form>
    <p class="mt-6 text-sm text-gray-600">
      Already have an account? <router-link to="/login" class="text-indigo-600 hover:underline">Login</router-link>
    </p>
  </div>
</template>
