<script lang="ts">
  import { onMount } from 'svelte'
  import { auth, authError, clearAuthError, login } from '../lib/stores/auth.js'
  import {
    items,
    totalPages,
    totalCount,
    pageIndex,
    pageSize,
    statusFilter,
    startDateFilter,
    endDateFilter,
    minPriceFilter,
    maxPriceFilter,
    searchQuery,
    loading,
    showingText,
    filterErrors,
    load,
    initRents,
    resetToFirstPage,
  } from '../lib/composables/rent/useRents.js'
  import { formatCurrency, formatDate } from '../lib/utils/index.js'

  let authChecked = false
  let loginFailed = false

  async function ensureAuth(): Promise<void> {
    if (!$auth?.token) {
      try {
        await login()
      } catch {
        loginFailed = true
        return
      }
    }
    loginFailed = false
    authChecked = true
  }

  function applyFilters(): void {
    resetToFirstPage()
    load()
  }

  onMount(async () => {
    initRents(10)
    await ensureAuth()
    if (!loginFailed) await load()
  })

  function goPage(p: number): void {
    if (p < 1 || p > $totalPages) return
    pageIndex.set(p)
    load()
  }
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">My rentals</h1>
  {#if $filterErrors?.general}
    <div class="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700" role="alert">
      {$filterErrors.general}
    </div>
  {/if}
  <div class="flex flex-wrap items-end gap-4">
    <div class="min-w-[200px] max-w-xs">
      <label for="rentals-search" class="mb-1 block text-sm font-medium text-gray-700">Search</label>
      <input
        id="rentals-search"
        type="search"
        placeholder="Vehicle or plate…"
        bind:value={$searchQuery}
        on:keydown={(e) => e.key === 'Enter' && applyFilters()}
        class="w-full rounded-md border border-gray-300 px-3 py-2 text-sm"
      />
    </div>
    <div class="min-w-[140px]">
      <label for="rentals-status" class="mb-1 block text-sm font-medium text-gray-700">Status</label>
      <select
        id="rentals-status"
        bind:value={$statusFilter}
        class="w-full rounded-md border border-gray-300 px-3 py-2 text-sm"
      >
        <option value={undefined}>All</option>
        <option value="Pending">Pending</option>
        <option value="Active">Active</option>
        <option value="Completed">Completed</option>
        <option value="Cancelled">Cancelled</option>
      </select>
    </div>
    <div class="min-w-[140px]">
      <label for="start-date" class="mb-1 block text-sm font-medium text-gray-700">From date</label>
      <input
        id="start-date"
        type="date"
        bind:value={$startDateFilter}
        class="w-full rounded-md border px-3 py-2 text-sm"
        class:border-red-500={$filterErrors?.startDateFrom}
      />
      {#if $filterErrors?.startDateFrom}
        <p class="mt-1 text-sm text-red-600">{$filterErrors.startDateFrom}</p>
      {/if}
    </div>
    <div class="min-w-[140px]">
      <label for="end-date" class="mb-1 block text-sm font-medium text-gray-700">To date</label>
      <input
        id="end-date"
        type="date"
        bind:value={$endDateFilter}
        class="w-full rounded-md border px-3 py-2 text-sm"
        class:border-red-500={$filterErrors?.startDateTo}
      />
      {#if $filterErrors?.startDateTo}
        <p class="mt-1 text-sm text-red-600">{$filterErrors.startDateTo}</p>
      {/if}
    </div>
    <div>
      <label for="min-price" class="mb-1 block text-sm font-medium text-gray-700">Min price</label>
      <input
        id="min-price"
        type="number"
        min="0"
        step="0.01"
        placeholder="0"
        bind:value={$minPriceFilter}
        class="w-28 rounded-md border px-3 py-2 text-sm"
        class:border-red-500={$filterErrors?.minPrice}
      />
      {#if $filterErrors?.minPrice}
        <p class="mt-1 text-sm text-red-600">{$filterErrors.minPrice}</p>
      {/if}
    </div>
    <div>
      <label for="max-price" class="mb-1 block text-sm font-medium text-gray-700">Max price</label>
      <input
        id="max-price"
        type="number"
        min="0"
        step="0.01"
        placeholder="Any"
        bind:value={$maxPriceFilter}
        class="w-28 rounded-md border px-3 py-2 text-sm"
        class:border-red-500={$filterErrors?.maxPrice}
      />
      {#if $filterErrors?.maxPrice}
        <p class="mt-1 text-sm text-red-600">{$filterErrors.maxPrice}</p>
      {/if}
    </div>
    <button
      type="button"
      class="rounded-md bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700"
      on:click={applyFilters}
    >
      Apply filters
    </button>
  </div>
  {#if loginFailed && $authError}
    <div class="rounded-md bg-red-50 p-3 text-sm text-red-700 flex flex-col gap-2">
      <span>Login failed. {$authError}</span>
      <button
        type="button"
        class="self-start rounded-md border border-red-300 px-3 py-1.5 text-sm text-red-700 hover:bg-red-100"
        on:click={() => { clearAuthError(); loginFailed = false; ensureAuth().then(() => load()); }}
      >
        Retry login
      </button>
    </div>
  {:else if $loading && !authChecked}
    <p class="py-8 text-center text-gray-500">Logging in…</p>
  {:else if $loading}
    <p class="py-8 text-center text-gray-500">Loading…</p>
  {:else if $items.length === 0}
    <p class="py-8 text-center text-gray-500">
      No rentals yet. <a href="#/book" class="text-indigo-600 hover:underline">Book a car</a>.
    </p>
  {:else}
    {#if $showingText}
      <p class="text-sm text-gray-600">{$showingText}</p>
    {/if}
    <div class="overflow-x-auto">
      <table class="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Vehicle</th>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">License plate</th>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Start</th>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">End</th>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Total</th>
            <th class="border-b border-gray-200 px-3 py-2 text-left font-semibold">Status</th>
          </tr>
        </thead>
        <tbody>
          {#each $items as r}
            <tr class="border-b border-gray-200">
              <td class="px-3 py-2">{r.vehicleModel}</td>
              <td class="px-3 py-2">{r.licensePlate}</td>
              <td class="px-3 py-2">{formatDate(r.startDate)}</td>
              <td class="px-3 py-2">{formatDate(r.endDate)}</td>
              <td class="px-3 py-2">{formatCurrency(r.totalPrice)}</td>
              <td class="px-3 py-2">{r.status}</td>
            </tr>
          {/each}
        </tbody>
      </table>
    </div>
    {#if $totalPages > 1}
      <div class="mt-4 flex items-center justify-center gap-2">
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          disabled={$pageIndex <= 1}
          on:click={() => goPage($pageIndex - 1)}
        >
          Previous
        </button>
        <span class="text-sm">Page {$pageIndex} of {$totalPages}</span>
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          disabled={$pageIndex >= $totalPages}
          on:click={() => goPage($pageIndex + 1)}
        >
          Next
        </button>
      </div>
    {/if}
  {/if}
</div>
