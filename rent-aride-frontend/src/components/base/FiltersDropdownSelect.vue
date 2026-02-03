<script setup lang="ts">
import { ref } from 'vue'

interface OptionItem {
  id: number
  name: string
}

const props = withDefaults(
  defineProps<{
    modelValue: number | undefined
    options: OptionItem[]
    label?: string
    placeholder?: string
    allOptionLabel?: string
  }>(),
  {
    label: '',
    placeholder: 'Select…',
    allOptionLabel: 'All',
  }
)

const emit = defineEmits<{
  'update:modelValue': [value: number | undefined]
}>()

const selectId = ref(`filters-dropdown-${Math.random().toString(36).slice(2, 9)}`)

function onInput(e: Event) {
  const value = (e.target as HTMLSelectElement).value
  emit('update:modelValue', value === '' ? undefined : Number(value))
}
</script>

<template>
  <div class="flex flex-col gap-1">
    <label v-if="label" :for="selectId" class="text-sm font-medium text-gray-700 dark:text-gray-300">
      {{ label }}
    </label>
    <select
      :id="selectId"
      :value="modelValue ?? ''"
      class="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
      @change="onInput"
    >
      <option value="">{{ allOptionLabel }}</option>
      <option
        v-for="opt in options"
        :key="opt.id"
        :value="opt.id"
      >
        {{ opt.name }}
      </option>
    </select>
  </div>
</template>
