<script lang="ts">
  import { onMount } from 'svelte'
  import { push } from 'svelte-spa-router'
  import {
    types,
    vehicles,
    totalPages,
    pageIndex,
    pageSize,
    vehicleTypeId,
    searchQuery,
    loading,
    loadingTypes,
    loadTypes,
    loadVehicles,
    initVehicles,
  } from '../lib/vehicles.js'
  import { formatCurrency } from '../lib/utils/index.js'
  import type { VehicleDto } from '../lib/types/vehicle.js'

  onMount(() => initVehicles(10))

  $: if (!$loadingTypes) {
    $vehicleTypeId
    $pageIndex
    $searchQuery
    loadVehicles()
  }

  function book(vehicle: VehicleDto) {
    push('/book?vehicleId=' + vehicle.id)
  }
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">Browse vehicles</h1>
  <div class="flex gap-4 items-center flex-wrap">
    <label for="type" class="text-sm">Vehicle type</label>
    <select
      id="type"
      bind:value={$vehicleTypeId}
      class="rounded-md border border-gray-300 px-3 py-2 text-sm flex-1 min-w-[140px]"
    >
      <option value="">All</option>
      {#each $types as t}
        <option value={t.id}>{t.name}</option>
      {/each}
    </select>
  </div>
  {#if $loading}
    <p class="py-8 text-center text-gray-500">Loading…</p>
  {:else if $vehicles.length === 0}
    <p class="py-8 text-center text-gray-500">No vehicles found.</p>
  {:else}
    <div class="grid gap-4">
      {#each $vehicles as v}
        <article
          class="rounded-lg border border-gray-200 bg-white p-4 shadow-sm"
        >
          <h3 class="font-medium">{v.model} ({v.year})</h3>
          <p class="text-sm text-gray-500">{v.vehicleTypeName} · {v.licensePlate}</p>
          <p class="mt-2 text-lg font-semibold">{formatCurrency(v.dailyPrice)} <span class="text-sm font-normal">/ day</span></p>
          <p class="text-sm">Status: {v.status}</p>
          <button
            type="button"
            class="mt-3 w-full rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={v.status !== 'Available'}
            on:click={() => book(v)}
          >
            Book
          </button>
        </article>
      {/each}
    </div>
    {#if $totalPages > 1}
      <div class="flex items-center justify-center gap-4">
        <button
          type="button"
          class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
          disabled={$pageIndex <= 1}
          on:click={() => { pageIndex.update((n) => Math.max(1, n - 1)); loadVehicles(); }}
        >
          Previous
        </button>
        <span class="text-sm">Page {$pageIndex} of {$totalPages}</span>
        <button
          type="button"
          class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100 disabled:opacity-50"
          disabled={$pageIndex >= $totalPages}
          on:click={() => { pageIndex.update((n) => n + 1); loadVehicles(); }}
        >
          Next
        </button>
      </div>
    {/if}
  {/if}
</div>
