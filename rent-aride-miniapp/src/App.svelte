<script lang="ts">
  import { onMount } from 'svelte'
  import Router from 'svelte-spa-router'
  import Home from './routes/Home.svelte'
  import Book from './routes/Book.svelte'
  import Rentals from './routes/Rentals.svelte'
  import PaymentFinish from './routes/PaymentFinish.svelte'
  import Debug from './routes/Debug.svelte'
  import { auth, authError, clearAuthError, logout } from './lib/stores/auth.js'


  const routes = {
    '/': Home,
    '/book': Book,
    '/rentals': Rentals,
    '/payment/finish': PaymentFinish,
    '/debug': Debug,
  }
</script>

<div class="min-h-screen flex flex-col">
  {#if $authError}
    <div class="bg-red-100 border-b border-red-200 px-4 py-2 flex items-center justify-between gap-2">
      <span class="text-sm text-red-800">Login failed: {$authError}</span>
      <button
        type="button"
        class="shrink-0 text-red-600 hover:text-red-800 text-sm font-medium"
        on:click={clearAuthError}
        aria-label="Dismiss"
      >
        Dismiss
      </button>
    </div>
  {/if}

  <header class="h-16 w-full bg-stone-700 flex items-center justify-between px-4 fixed top-0 left-0 right-0 z-10">
    <a href="#/" class="text-xl font-bold text-white hover:no-underline">Rent-A-Ride</a>
    <nav class="flex items-center gap-3">
      <a href="#/" class="text-white/90 hover:text-white text-sm">Browse</a>
      <a href="#/book" class="text-white/90 hover:text-white text-sm">Book</a>
      <a href="#/rentals" class="text-white/90 hover:text-white text-sm">My rentals</a>
      <a href="#/debug" class="text-white/90 hover:text-white text-sm">Debug</a>
      {#if $auth?.token}
        <button
          type="button"
          class="rounded border border-white/50 px-3 py-1 text-sm text-white hover:bg-white/10"
          on:click={logout}
        >
          Logout
        </button>
      {/if}
    </nav>
  </header>

  <main class="flex-1 w-full max-w-2xl mx-auto">
    <Router {routes} />
  </main>
</div>
