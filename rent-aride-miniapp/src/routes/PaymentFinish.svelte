<script lang="ts">
  import { push } from 'svelte-spa-router'
  import {
    getPaymentFinishQuery,
    getStatusLabel,
    isSuccessStatus,
    isFailedStatus,
  } from '../lib/composables/payment/usePaymentFinish.js'

  const { requestId, paymentId, paymentType, status } = getPaymentFinishQuery()
  const statusLabel = getStatusLabel(status)
  const isSuccess = isSuccessStatus(status)
  const isFailed = isFailedStatus(status)

  function goToRentals(): void {
    push('/rentals')
  }

  function goHome(): void {
    push('/')
  }
</script>

<div class="p-4 mx-auto max-w-lg space-y-6">
  <div
    class="rounded-xl border bg-white p-6 shadow-sm"
    class:border-emerald-200={isSuccess}
    class:border-rose-200={isFailed}
    class:border-gray-200={!isSuccess && !isFailed}
  >
    <div class="flex flex-col items-center gap-4 text-center">
      <div
        class="flex size-14 items-center justify-center rounded-full"
        class:bg-emerald-100={isSuccess}
        class:bg-rose-100={isFailed}
        class:bg-gray-100={!isSuccess && !isFailed}
      >
        {#if isSuccess}
          <svg class="size-8 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
          </svg>
        {:else if isFailed}
          <svg class="size-8 text-rose-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        {:else}
          <svg class="size-8 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        {/if}
      </div>
      <div>
        <h1
          class="text-xl font-semibold"
          class:text-emerald-700={isSuccess}
          class:text-rose-700={isFailed}
          class:text-gray-900={!isSuccess && !isFailed}
        >
          {statusLabel}
        </h1>
        <p class="mt-1 text-sm text-gray-600">
          {#if isSuccess}
            Thank you for your payment. Your rental will appear in My Rentals once confirmed.
          {:else if isFailed}
            Your payment could not be completed. You can try again from My Rentals or book a new vehicle.
          {:else}
            You were redirected from the payment provider. Check the details below.
          {/if}
        </p>
      </div>
    </div>

    {#if requestId || paymentId || paymentType || status}
      <dl class="mt-6 space-y-3 border-t border-gray-200 pt-4">
        {#if requestId}
          <div class="flex justify-between gap-4 text-sm">
            <dt class="text-gray-500">Request ID</dt>
            <dd class="truncate font-mono text-gray-900" title={requestId}>{requestId}</dd>
          </div>
        {/if}
        {#if paymentId}
          <div class="flex justify-between gap-4 text-sm">
            <dt class="text-gray-500">Payment ID</dt>
            <dd class="truncate font-mono text-gray-900" title={paymentId}>{paymentId}</dd>
          </div>
        {/if}
        {#if paymentType}
          <div class="flex justify-between gap-4 text-sm">
            <dt class="text-gray-500">Payment type</dt>
            <dd class="font-medium text-gray-900">{paymentType}</dd>
          </div>
        {/if}
        {#if status}
          <div class="flex justify-between gap-4 text-sm">
            <dt class="text-gray-500">Status</dt>
            <dd>
              <span
                class="inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium"
                class:bg-emerald-100={isSuccess}
                class:text-emerald-800={isSuccess}
                class:bg-rose-100={isFailed}
                class:text-rose-800={isFailed}
                class:bg-gray-100={!isSuccess && !isFailed}
                class:text-gray-800={!isSuccess && !isFailed}
              >
                {status}
              </span>
            </dd>
          </div>
        {/if}
      </dl>
    {/if}
  </div>

  <div class="flex flex-wrap justify-center gap-3">
    <button
      type="button"
      class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
      on:click={goToRentals}
    >
      View my rentals
    </button>
    <button
      type="button"
      class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
      on:click={goHome}
    >
      Browse vehicles
    </button>
  </div>
</div>
