<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()
</script>

<template>
  <div class="flex flex-col items-center justify-center py-16 text-center">
    <h1 class="text-2xl font-bold text-gray-900 dark:text-gray-100">Page not found</h1>
    <p class="mt-2 text-gray-600 dark:text-gray-400">The page you're looking for doesn't exist.</p>
    <button
      type="button"
      class="mt-6 rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
      @click="router.push('/')"
    >
      Go home
    </button>
  </div>
</template>
