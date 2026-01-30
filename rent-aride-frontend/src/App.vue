<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { storeToRefs } from 'pinia'

const auth = useAuthStore()
const { user, isLoggedIn, isAdmin } = storeToRefs(auth)
</script>

<template>
  <div class="app">
    <header class="header">
      <RouterLink to="/" class="brand">Rent-A-Ride</RouterLink>
      <nav class="nav">
        <RouterLink to="/">Browse</RouterLink>
        <template v-if="isLoggedIn">
          <RouterLink to="/rentals/new">Book a car</RouterLink>
          <RouterLink to="/rentals">My rentals</RouterLink>
          <template v-if="isAdmin">
            <RouterLink to="/admin/vehicle-types">Vehicle types</RouterLink>
            <RouterLink to="/admin/amenities">Amenities</RouterLink>
            <RouterLink to="/admin/vehicles">Vehicles</RouterLink>
          </template>
          <span class="user">{{ user?.email }}</span>
          <button type="button" class="btn btn-outline" @click="auth.logout()">Logout</button>
        </template>
        <template v-else>
          <RouterLink to="/login">Login</RouterLink>
          <RouterLink to="/register">Register</RouterLink>
        </template>
      </nav>
    </header>
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
.header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1.5rem;
  background: var(--color-background-soft);
  border-bottom: 1px solid var(--color-border);
}
.brand {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--color-heading);
  text-decoration: none;
}
.brand:hover {
  text-decoration: underline;
}
.nav {
  display: flex;
  align-items: center;
  gap: 1rem;
}
.nav a {
  color: var(--color-text);
  text-decoration: none;
  padding: 0.35rem 0.6rem;
  border-radius: 6px;
}
.nav a:hover {
  background: var(--color-background-mute);
}
.nav a.router-link-exact-active {
  font-weight: 600;
  background: var(--color-background-mute);
}
.user {
  font-size: 0.9rem;
  color: var(--color-text);
  opacity: 0.9;
  margin-left: 0.5rem;
}
.main {
  flex: 1;
  padding: 1.5rem;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
}
.btn {
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  font-size: 0.9rem;
  cursor: pointer;
  border: 1px solid var(--color-border);
  background: transparent;
  color: var(--color-text);
}
.btn-outline:hover {
  background: var(--color-background-mute);
}
</style>
