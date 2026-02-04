<script lang="ts">
  import { onMount } from 'svelte'
  import { push } from 'svelte-spa-router'
  import { auth, login } from '../lib/stores/auth.js'
  import {
    vehicles,
    amenities,
    selectedVehicleId,
    startDate,
    endDate,
    selectedAmenityIds,
    loading,
    loadingForm,
    generalError,
    initCreateRental,
    onSubmit,
    toggleAmenity,
  } from '../lib/composables/rent/useCreateRental.js'
  import { tradePayWithUrl, alert } from '../lib/bridge/index.js'
  import { formatCurrency } from '../lib/utils/index.js'

  async function ensureAuth(): Promise<void> {
    if (!$auth?.token) {
      await login()
    }
  }

  async function handleSubmit(): Promise<void> {
    const finishUrl = typeof window !== 'undefined' ? `${window.location.origin}/#/payment/finish` : ''
    await onSubmit(ensureAuth, finishUrl, async (paymentUrl: string) => {
      await tradePayWithUrl(paymentUrl)
      push('/payment/finish')
    }, (msg: string) => alert(msg))
  }

  onMount(() => {
    const q = typeof window !== 'undefined' ? window.location.hash.split('?')[1] || '' : ''
    const p = new URLSearchParams(q)
    const v = p.get('vehicleId')
    initCreateRental(v ? parseInt(v, 10) : null)
  })
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold text-gray-900">Book a car</h1>
  {#if $loadingForm}
    <p class="py-8 text-center">Loading…</p>
  {:else}
    <form on:submit|preventDefault={handleSubmit} class="flex flex-col gap-4 max-w-md rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      {#if $generalError}
        <div class="rounded-lg border border-rose-200 bg-rose-50 p-3 text-sm text-rose-700">
          {$generalError}
        </div>
      {/if}
      <div>
        <label for="vehicle" class="mb-1 block text-sm font-medium text-gray-700">Vehicle</label>
        <select
          id="vehicle"
          bind:value={$selectedVehicleId}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        >
          {#each $vehicles as v}
            <option value={v.id}>{v.model} ({v.year}) — {formatCurrency(v.dailyPrice)}/day</option>
          {/each}
        </select>
      </div>
      <div>
        <label for="start" class="mb-1 block text-sm font-medium text-gray-700">Start date</label>
        <input
          id="start"
          type="date"
          bind:value={$startDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      <div>
        <label for="end" class="mb-1 block text-sm font-medium text-gray-700">End date</label>
        <input
          id="end"
          type="date"
          bind:value={$endDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      {#if $amenities.length > 0}
        <div role="group" aria-labelledby="extras-heading">
          <span id="extras-heading" class="mb-1 block text-sm font-medium text-gray-700">Extras</span>
          <div class="flex flex-wrap gap-4">
            {#each $amenities as a}
              <label class="flex cursor-pointer items-center gap-2 text-sm text-gray-700">
                <input
                  type="checkbox"
                  checked={$selectedAmenityIds.includes(a.id)}
                  on:change={() => toggleAmenity(a.id)}
                  class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
                />
                {a.name} <span class="font-medium text-indigo-600">(+{formatCurrency(a.price)})</span>
              </label>
            {/each}
          </div>
        </div>
      {/if}
      <div class="flex gap-3 pt-2">
        <a href="#/" class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50">Cancel</a>
        <button
          type="submit"
          class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-60"
          disabled={$loading}
        >
          {$loading ? 'Redirecting to payment…' : 'Proceed to payment'}
        </button>
      </div>
    </form>
  {/if}
</div>
