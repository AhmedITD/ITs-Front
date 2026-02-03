<script setup lang="ts">
import FiltersDropdownSelect from '@/components/base/FiltersDropdownSelect.vue'
import type { VehicleTypeDto } from '@/types/vehicle'

defineProps<{
  modelValue: number | undefined
  options: VehicleTypeDto[]
  label?: string
  placeholder?: string
}>()
const emit = defineEmits<{
  'update:modelValue': [value: number | undefined]
}>()
</script>

<template>
  <FiltersDropdownSelect
    :model-value="modelValue"
    :options="options"
    :label="label ?? 'Vehicle type'"
    :placeholder="placeholder ?? 'Select vehicle type'"
    all-option-label="All"
    @update:model-value="emit('update:modelValue', $event)"
  />
</template>
