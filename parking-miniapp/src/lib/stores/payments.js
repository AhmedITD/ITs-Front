import { writable } from 'svelte/store';

const PAYMENTS_KEY = 'parking_miniapp_payments';

function loadPayments() {
  try {
    const s = localStorage.getItem(PAYMENTS_KEY);
    if (s) return JSON.parse(s);
  } catch (_) {}
  return [];
}

export const payments = writable(loadPayments());

payments.subscribe((v) => {
  localStorage.setItem(PAYMENTS_KEY, JSON.stringify(v));
});

export function addPayment(record) {
  payments.update((p) => [
    {
      ...record,
      id: 'pay-' + Date.now(),
      createdAt: new Date().toISOString(),
    },
    ...p,
  ]);
}
