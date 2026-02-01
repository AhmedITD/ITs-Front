<script>
  import { onMount } from 'svelte';
  import { push } from 'svelte-spa-router';
  import { querystring } from 'svelte-spa-router';
  import { auth, login } from '../lib/stores/auth.js';
  import * as vehiclesApi from '../lib/api/vehicles.js';
  import * as amenitiesApi from '../lib/api/amenities.js';
  import * as rentalsApi from '../lib/api/rentals.js';
  import { tradePayWithUrl, alert } from '../lib/bridge/index.js';
  import { formatCurrency } from '../lib/utils.js';

  $: params = (() => {
    const p = new URLSearchParams($querystring || '');
    return { vehicleId: p.get('vehicleId') ? parseInt(p.get('vehicleId'), 10) : null };
  })();

  let vehicles = [];
  let amenities = [];
  let selectedVehicleId = $params.vehicleId;
  let startDate = '';
  let endDate = '';
  let selectedAmenityIds = [];
  let loading = false;
  let loadingForm = true;
  let generalError = '';

  async function loadFormData() {
    loadingForm = true;
    try {
      const [vRes, aRes] = await Promise.all([
        vehiclesApi.browse({ pageSize: 500 }),
        amenitiesApi.getAll(),
      ]);
      vehicles = (vRes.items ?? []).filter((v) => v.status === 'Available');
      amenities = Array.isArray(aRes) ? aRes : (aRes ?? []);
      if (params.vehicleId && vehicles.some((v) => v.id === params.vehicleId)) {
        selectedVehicleId = params.vehicleId;
      } else if (vehicles.length && !selectedVehicleId) {
        selectedVehicleId = vehicles[0]?.id ?? null;
      }
    } catch (e) {
      generalError = e?.message || 'Failed to load form data';
    } finally {
      loadingForm = false;
    }
  }

  function toUtcIso(dateOnly, endOfDay) {
    return endOfDay ? `${dateOnly}T23:59:59.999Z` : `${dateOnly}T00:00:00.000Z`;
  }

  async function ensureAuth() {
    if (!$auth?.token) {
      await login();
    }
  }

  async function onSubmit() {
    if (!selectedVehicleId || !startDate || !endDate) {
      generalError = 'Please select a vehicle and dates.';
      return;
    }
    generalError = '';
    loading = true;
    try {
      await ensureAuth();
      const finishUrl = typeof window !== 'undefined' ? `${window.location.origin}/#/payment/finish` : '';
      const res = await rentalsApi.createInvoice({
        vehicleId: selectedVehicleId,
        startDate: toUtcIso(startDate, false),
        endDate: toUtcIso(endDate, true),
        amenityIds: selectedAmenityIds,
        finishUrl,
      });
      const paymentUrl = res?.paymentUrl ?? res?.data?.paymentUrl;
      if (paymentUrl) {
        await tradePayWithUrl(paymentUrl);
        push('/payment/finish');
      } else {
        generalError = res?.message || 'Failed to create payment link';
      }
    } catch (e) {
      generalError = e?.message || 'Something went wrong';
      alert(generalError);
    } finally {
      loading = false;
    }
  }

  function toggleAmenity(id) {
    const i = selectedAmenityIds.indexOf(id);
    if (i === -1) selectedAmenityIds = [...selectedAmenityIds, id];
    else selectedAmenityIds = selectedAmenityIds.filter((x) => x !== id);
  }

  onMount(loadFormData);
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">Book a car</h1>
  {#if loadingForm}
    <p class="py-8 text-center">Loading…</p>
  {:else}
    <form on:submit|preventDefault={onSubmit} class="flex flex-col gap-4 max-w-md">
      {#if generalError}
        <div class="rounded-md bg-red-50 p-3 text-sm text-red-700">{generalError}</div>
      {/if}
      <div>
        <label for="vehicle-select" class="mb-1 block text-sm font-medium">Vehicle</label>
        <select
          id="vehicle-select"
          bind:value={selectedVehicleId}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        >
          {#each vehicles as v}
            <option value={v.id}>{v.model} ({v.year}) — {formatCurrency(v.dailyPrice)}/day</option>
          {/each}
        </select>
      </div>
      <div>
        <label for="start" class="mb-1 block text-sm font-medium">Start date</label>
        <input
          id="start"
          type="date"
          bind:value={startDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      <div>
        <label for="end" class="mb-1 block text-sm font-medium">End date</label>
        <input
          id="end"
          type="date"
          bind:value={endDate}
          required
          class="w-full rounded-md border border-gray-300 px-3 py-2"
        />
      </div>
      {#if amenities.length > 0}
        <div>
          <span class="mb-1 block text-sm font-medium">Extras</span>
          <div class="flex flex-wrap gap-4">
            {#each amenities as a}
              <label class="flex cursor-pointer items-center gap-2">
                <input
                  type="checkbox"
                  checked={selectedAmenityIds.includes(a.id)}
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
          disabled={loading}
        >
          {loading ? 'Processing…' : 'Proceed to payment'}
        </button>
      </div>
    </form>
  {/if}
</div>
