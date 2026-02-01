<script>
  import { onMount } from 'svelte';
  import { auth, login } from '../lib/stores/auth.js';
  import * as rentalsApi from '../lib/api/rentals.js';
  import { formatCurrency, formatDate } from '../lib/utils.js';

  let items = [];
  let totalPages = 0;
  let totalCount = 0;
  let pageIndex = 1;
  let pageSize = 10;
  let loading = false;
  let authChecked = false;

  async function ensureAuth() {
    if (!$auth?.token) {
      await login();
    }
    authChecked = true;
  }

  async function load() {
    await ensureAuth();
    loading = true;
    try {
      const res = await rentalsApi.getMyHistory(pageIndex, pageSize);
      items = res.items ?? [];
      totalPages = res.totalPages ?? 0;
      totalCount = res.totalCount ?? 0;
    } catch (e) {
      items = [];
    } finally {
      loading = false;
    }
  }

  function goPage(p) {
    if (p < 1 || p > totalPages) return;
    pageIndex = p;
    load();
  }

  onMount(load);
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">My rentals</h1>
  {#if loading && !authChecked}
    <p class="py-8 text-center text-gray-500">Logging in…</p>
  {:else if loading}
    <p class="py-8 text-center text-gray-500">Loading…</p>
  {:else if items.length === 0}
    <p class="py-8 text-center text-gray-500">
      No rentals yet. <a href="#/book" class="text-indigo-600 hover:underline">Book a car</a>.
    </p>
  {:else}
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
          {#each items as r}
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
    {#if totalPages > 1}
      <div class="mt-4 flex items-center justify-center gap-2">
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          disabled={pageIndex <= 1}
          on:click={() => goPage(pageIndex - 1)}
        >
          Previous
        </button>
        <span class="text-sm">Page {pageIndex} of {totalPages}</span>
        <button
          type="button"
          class="rounded-md border border-gray-300 px-3 py-1.5 text-sm hover:bg-gray-100 disabled:opacity-50"
          disabled={pageIndex >= totalPages}
          on:click={() => goPage(pageIndex + 1)}
        >
          Next
        </button>
      </div>
    {/if}
  {/if}
</div>
