<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { rentalsApi, type RentalHistoryItemDto } from '@/api/rentals'

const items = ref<RentalHistoryItemDto[]>([])
const totalPages = ref(0)
const pageIndex = ref(1)
const pageSize = 10
const loading = ref(false)

async function load() {
  loading.value = true
  try {
    const { data } = await rentalsApi.getMyHistory(pageIndex.value, pageSize)
    if (data.success && data.data) {
      items.value = data.data.items
      totalPages.value = data.data.totalPages
    }
  } finally {
    loading.value = false
  }
}

function formatDate(s: string) {
  return new Date(s).toLocaleDateString()
}

onMounted(load)

function goPage(p: number) {
  pageIndex.value = p
  load()
}
</script>

<template>
  <div class="my-rentals">
    <h1>My rentals</h1>
    <div v-if="loading" class="loading">Loading…</div>
    <div v-else-if="items.length === 0" class="empty">No rentals yet. <router-link to="/rentals/new">Book a car</router-link>.</div>
    <div v-else>
      <table class="table">
        <thead>
          <tr>
            <th>Vehicle</th>
            <th>License plate</th>
            <th>Start</th>
            <th>End</th>
            <th>Total</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="r in items" :key="r.id">
            <td>{{ r.vehicleModel }}</td>
            <td>{{ r.licensePlate }}</td>
            <td>{{ formatDate(r.startDate) }}</td>
            <td>{{ formatDate(r.endDate) }}</td>
            <td>${{ r.totalPrice.toFixed(2) }}</td>
            <td>{{ r.status }}</td>
          </tr>
        </tbody>
      </table>
      <div v-if="totalPages > 1" class="pagination">
        <button type="button" class="btn" :disabled="pageIndex <= 1" @click="goPage(pageIndex - 1)">Previous</button>
        <span class="page-info">Page {{ pageIndex }} of {{ totalPages }}</span>
        <button type="button" class="btn" :disabled="pageIndex >= totalPages" @click="goPage(pageIndex + 1)">Next</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.my-rentals h1 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}
.loading,
.empty {
  padding: 2rem;
  text-align: center;
  color: var(--color-text);
}
.empty a {
  color: var(--vt-c-indigo);
}
.table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}
.table th,
.table td {
  padding: 0.6rem 0.75rem;
  text-align: left;
  border-bottom: 1px solid var(--color-border);
}
.table th {
  font-weight: 600;
  background: var(--color-background-mute);
}
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 1rem;
}
.page-info {
  font-size: 0.9rem;
}
.btn {
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  border: 1px solid var(--color-border);
  background: var(--color-background-soft);
  color: var(--color-text);
  cursor: pointer;
}
.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
