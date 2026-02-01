<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuth } from '@/composables/useAuth'

const { user, isLoggedIn, isAdmin, logout } = useAuth()
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <header class="flex items-center justify-between border-b border-gray-200 bg-gray-50 px-6 py-4 dark:border-gray-700 dark:bg-gray-800">
      <RouterLink to="/" class="text-xl font-bold text-gray-900 hover:underline dark:text-white">
        Rent-A-Ride
      </RouterLink>
      <nav class="flex items-center gap-4">
        <RouterLink
          to="/"
          class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
          active-class="font-semibold"
        >
          Browse
        </RouterLink>
        <template v-if="isLoggedIn">
          <RouterLink
            to="/rentals/new"
            class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
            active-class="font-semibold"
          >
            Book a car
          </RouterLink>
          <RouterLink
            to="/rentals"
            class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
            active-class="font-semibold"
          >
            My rentals
          </RouterLink>
          <template v-if="isAdmin">
            <RouterLink
              to="/admin/vehicle-types"
              class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
              active-class="font-semibold"
            >
              Vehicle types
            </RouterLink>
            <RouterLink
              to="/admin/amenities"
              class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
              active-class="font-semibold"
            >
              Amenities
            </RouterLink>
            <RouterLink
              to="/admin/vehicles"
              class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
              active-class="font-semibold"
            >
              Vehicles
            </RouterLink>
          </template>
          <span class="ml-2 text-sm text-gray-500">{{ user?.email }}</span>
          <button
            type="button"
            class="rounded border border-gray-300 px-4 py-2 text-sm hover:bg-gray-100 dark:border-gray-600 dark:hover:bg-gray-700"
            @click="logout()"
          >
            Logout
          </button>
        </template>
        <template v-else>
          <RouterLink
            to="/login"
            class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
            active-class="font-semibold"
          >
            Login
          </RouterLink>
          <RouterLink
            to="/register"
            class="rounded px-3 py-2 text-gray-600 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
            active-class="font-semibold"
          >
            Register
          </RouterLink>
        </template>
      </nav>
    </header>
    <main class="mx-auto w-full max-w-6xl flex-1 px-6 py-6">
      <RouterView />
    </main>
  </div>
</template>
