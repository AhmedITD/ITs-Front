<script setup lang="ts">
import { get, generalError, email, password, loading, handleSubmit } from '@/composables/Auth/useLogin'


</script>

<template>
  <div class="mx-auto max-w-sm space-y-6 py-8">
    <div class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-600 dark:bg-gray-800">
      <h1 class="mb-6 text-xl font-semibold text-gray-900 dark:text-gray-100">Login</h1>
      <form class="flex flex-col gap-4" @submit.prevent="handleSubmit">
        <div v-if="generalError && !get('email') && !get('password')" class="rounded-lg border border-rose-200 bg-rose-50 p-3 text-sm text-rose-700 dark:border-rose-800 dark:bg-rose-900/20 dark:text-rose-300">
          {{ generalError }}
        </div>
        <div>
          <label for="email" class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Email</label>
          <input
            id="email"
            v-model="email"
            type="email"
            required
            autocomplete="email"
            class="w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
            :class="get('email') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('email')" class="mt-1 text-sm text-rose-600 dark:text-rose-400">{{ get('email') }}</p>
        </div>
        <div>
          <label for="password" class="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Password</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            autocomplete="current-password"
            class="w-full rounded-lg border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-700 dark:text-gray-100"
            :class="get('password') ? 'border-rose-500 dark:border-rose-500' : ''"
          />
          <p v-if="get('password')" class="mt-1 text-sm text-rose-600 dark:text-rose-400">{{ get('password') }}</p>
        </div>
        <button
          type="submit"
          class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-60 dark:focus:ring-offset-gray-800"
          :disabled="loading"
        >
          {{ loading ? 'Signing in…' : 'Sign in' }}
        </button>
      </form>
    </div>
    <p class="text-center text-sm text-gray-600 dark:text-gray-400">
      Don't have an account? <router-link to="/register" class="font-medium text-indigo-600 hover:underline dark:text-indigo-400">Register</router-link>
    </p>
  </div>
</template>
