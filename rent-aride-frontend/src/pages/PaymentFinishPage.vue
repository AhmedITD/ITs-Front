<script setup lang="ts">
import usePaymentFinish from '@/composables/Payment/usePaymentFinish'

const {
  requestId,
  paymentId,
  paymentType,  
  statusLabel,
  status,
  isSuccess,
  isFailed,  
  goToRentals,
  goHome,
} = usePaymentFinish()

</script>

<template>
  <div class="mx-auto max-w-lg space-y-6">
    <div
      class="rounded-xl border bg-white p-6 shadow-sm dark:border-gray-600 dark:bg-gray-800"
      :class="
        isSuccess
          ? 'border-emerald-200 dark:border-emerald-800'
          : isFailed
            ? 'border-rose-200 dark:border-rose-800'
            : 'border-gray-200'
      "
    >
      <div class="flex flex-col items-center gap-4 text-center">
        <div
          class="flex size-14 items-center justify-center rounded-full"
          :class="
            isSuccess
              ? 'bg-emerald-100 dark:bg-emerald-900/40'
              : isFailed
                ? 'bg-rose-100 dark:bg-rose-900/40'
                : 'bg-gray-100 dark:bg-gray-700'
          "
        >
          <svg
            v-if="isSuccess"
            class="size-8 text-emerald-600 dark:text-emerald-400"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
          </svg>
          <svg
            v-else-if="isFailed"
            class="size-8 text-rose-600 dark:text-rose-400"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
          <svg
            v-else
            class="size-8 text-gray-500 dark:text-gray-400"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </div>
        <div>
          <h1
            class="text-xl font-semibold"
            :class="
              isSuccess
                ? 'text-emerald-700 dark:text-emerald-400'
                : isFailed
                  ? 'text-rose-700 dark:text-rose-400'
                  : 'text-gray-900 dark:text-gray-100'
            "
          >
            {{ statusLabel }}
          </h1>
          <p class="mt-1 text-sm text-gray-600 dark:text-gray-400">
            <template v-if="isSuccess">
              Thank you for your payment. Your rental will appear in My Rentals once confirmed.
            </template>
            <template v-else-if="isFailed">
              Your payment could not be completed. You can try again from My Rentals or book a new vehicle.
            </template>
            <template v-else>
              You were redirected from the payment provider. Check the details below.
            </template>
          </p>
        </div>
      </div>

      <dl v-if="requestId || paymentId || paymentType || status" class="mt-6 space-y-3 border-t border-gray-200 pt-4 dark:border-gray-600">
        <div v-if="requestId" class="flex justify-between gap-4 text-sm">
          <dt class="text-gray-500 dark:text-gray-400">Request ID</dt>
          <dd class="truncate font-mono text-gray-900 dark:text-gray-100" :title="requestId">{{ requestId }}</dd>
        </div>
        <div v-if="paymentId" class="flex justify-between gap-4 text-sm">
          <dt class="text-gray-500 dark:text-gray-400">Payment ID</dt>
          <dd class="truncate font-mono text-gray-900 dark:text-gray-100" :title="paymentId">{{ paymentId }}</dd>
        </div>
        <div v-if="paymentType" class="flex justify-between gap-4 text-sm">
          <dt class="text-gray-500 dark:text-gray-400">Payment type</dt>
          <dd class="font-medium text-gray-900 dark:text-gray-100">{{ paymentType }}</dd>
        </div>
        <div v-if="status" class="flex justify-between gap-4 text-sm">
          <dt class="text-gray-500 dark:text-gray-400">Status</dt>
          <dd>
            <span
              class="inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium"
              :class="
                isSuccess
                  ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-900/40 dark:text-emerald-300'
                  : isFailed
                    ? 'bg-rose-100 text-rose-800 dark:bg-rose-900/40 dark:text-rose-300'
                    : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
              "
            >
              {{ status }}
            </span>
          </dd>
        </div>
      </dl>
    </div>

    <div class="flex flex-wrap justify-center gap-3">
      <button
        type="button"
        class="rounded-lg bg-indigo-600 px-4 py-2.5 font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
        @click="goToRentals()"
      >
        View my rentals
      </button>
      <button
        type="button"
        class="rounded-lg border border-gray-300 bg-white px-4 py-2.5 font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-200 dark:hover:bg-gray-700 dark:focus:ring-offset-gray-800"
        @click="goHome()"
      >
        Browse vehicles
      </button>
    </div>
  </div>
</template>
