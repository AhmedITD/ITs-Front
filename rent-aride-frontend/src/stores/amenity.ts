import { defineStore } from 'pinia'
import { ref } from 'vue'
import { amenityService } from '@/servers/services/AmenityService'
import type { AmenityDto } from '@/types/amenity'

export const useAmenityStore = defineStore('amenity', () => {
  const amenities = ref<AmenityDto[]>([])
  const loading = ref(false)
  const loadingList = ref(false)

  async function load() {
    loadingList.value = true
    try {
      const res = await amenityService.getAll()
      if (res.data) amenities.value = res.data
    } finally {
      loadingList.value = false
    }
  }

  async function create(data: { name: string; price: number }) {
    const res = await amenityService.create(data)
    if (!res.data) throw new Error(res.message || 'Failed to create')
    await load()
    return res
  }

  async function update(id: number, data: { name: string; price: number }) {
    const res = await amenityService.update(id, data)
    if (!res.data) throw new Error(res.message || 'Failed to update')
    await load()
    return res
  }

  async function remove(id: number) {
    await amenityService.delete(id)
    await load()
  }

  return {
    amenities,
    loading,
    loadingList,
    load,
    create,
    update,
    remove,
  }
})
