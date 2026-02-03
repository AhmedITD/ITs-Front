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
    loadFormData,
    onSubmit,
    toggleAmenity,
  } from '../lib/createRental.js'
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
    loadFormData(v ? parseInt(v, 10) : null)
  })
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">Book a car</h1>
  {#if $loadingForm}
    <p class="py-8 text-center">Loading…</p>
  {:else}
    <form on:submit|preventDefault={handleSubmit} class="flex flex-col gap-4 max-w-md">
      
      <div>
        <label for="start" class="mb-1 block text-sm font-medium">Start date</label>
        <input
          id="start"
          type="date"
          bind:value={$startDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      <div>
        <label for="end" class="mb-1 block text-sm font-medium">End date</label>
        <input
          id="end"
          type="date"
          bind:value={$endDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      {#if $amenities.length > 0}
        <div>
          <span class="mb-1 block text-sm font-medium">Extras</span>
          <div class="flex flex-wrap gap-4">
            {#each $amenities as a}
              <label class="flex cursor-pointer items-center gap-2">
                <input
                  type="checkbox"
                  checked={$selectedAmenityIds.includes(a.id)}
                  on:change={() => toggleAmenity(a.id)}
                />
                {a.name} (+{formatCurrency(a.price)})
              </label>
            {/each}
          </div>
        </div>
      {/if}
      <div class="flex gap-3">
        <a href="#/" class="rounded-md border border-gray-300 px-4 py-2 hover:bg-gray-100">Cancel</a>
        <button
          type="submit"
          class="rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700 disabled:opacity-60"
          disabled={$loading}
        >
          {$loading ? 'Processing…' : 'Proceed to payment'}
        </button>
      </div>
    </form>
  {/if}
</div>
