<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()
</script>

<template>
  <div class="flex flex-col items-center justify-center py-16">
    <h1 class="text-2xl font-bold text-gray-900 dark:text-white">Page not found</h1>
    <p class="mt-2 text-gray-600 dark:text-gray-400">The page you're looking for doesn't exist.</p>
    <button
      type="button"
      class="mt-6 rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700"
      @click="router.push('/')"
    >
      Go home
    </button>
  </div>
</template>
