import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'

/**
 * Composable for payment finish page. Must be called from component setup so useRoute() runs in injection context.
*/
const route = useRoute()
const router = useRouter()
const requestId = computed(() => (route.query?.requestId as string) ?? '')
const paymentId = computed(() => (route.query?.paymentId as string) ?? '')
const paymentType = computed(() => (route.query?.paymentType as string) ?? '')
const status = computed(() => ((route.query?.status as string) ?? '').toUpperCase())

const statusLabel = computed(() => getStatusLabel(status.value))
const isSuccess = computed(() => isSuccessStatus(status.value))
const isFailed = computed(() => isFailedStatus(status.value))


function getStatusLabel(statusVal: string | undefined): string {
  switch (statusVal) {
    case 'SUCCESS':
      return 'Payment successful'
    case 'FAILED':
    case 'CANCELLED':
      return statusVal.charAt(0) + statusVal.slice(1).toLowerCase()
    default:
      return statusVal || 'Unknown'
  }
}

function isSuccessStatus(statusVal: string): boolean {
  return statusVal === 'SUCCESS'
}

function isFailedStatus(statusVal: string): boolean {
  return !!statusVal && statusVal !== 'SUCCESS'
}

function goToRentals() {
  router.push({ name: 'rentals' })
}

function goHome() {
  router.push({ name: 'home' })
}

export default function usePaymentFinish() {
  return {
    requestId,
    paymentId,
    paymentType,
    status,
    statusLabel,
    isSuccess,
    isFailed,
    getStatusLabel,
    isSuccessStatus,
    isFailedStatus,
    goToRentals,
    goHome,
  }
}