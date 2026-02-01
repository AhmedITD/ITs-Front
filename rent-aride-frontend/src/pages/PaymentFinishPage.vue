<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()

function goToRentals() {
  router.push({ name: 'rentals' })
}
</script>

<template>
  <div class="space-y-4">
    <h1 class="text-xl font-semibold">Payment complete</h1>
    <p class="text-gray-600">
      Thank you for your payment. Your rental will appear in My Rentals once the payment is confirmed.
    </p>
    <div>
      <button
        type="button"
        class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700"
        @click="goToRentals"
      >
        View my rentals
      </button>
    </div>
  </div>
</template>
