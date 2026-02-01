export function formatCurrency(amount) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'IQD',
    maximumFractionDigits: 0,
    minimumFractionDigits: 0,
  }).format(amount ?? 0);
}

export function formatDate(iso) {
  return new Date(iso).toLocaleDateString();
}
