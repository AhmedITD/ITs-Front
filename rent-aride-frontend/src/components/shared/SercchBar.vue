<script setup lang="ts">
import { ref, watch } from 'vue'

const props = withDefaults(
  defineProps<{
    modelValue?: string
    placeholder?: string
    debounceMs?: number
  }>(),
  {
    modelValue: '',
    placeholder: 'Search…',
    debounceMs: 300,
  }
)

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const inputId = ref(`search-bar-${Math.random().toString(36).slice(2, 9)}`)
const local = ref(props.modelValue)
let debounceTimer: ReturnType<typeof setTimeout> | null = null

watch(
  () => props.modelValue,
  (v) => {
    local.value = v
  }
)

function onInput(e: Event) {
  const value = (e.target as HTMLInputElement).value
  local.value = value
  if (props.debounceMs > 0) {
    if (debounceTimer) clearTimeout(debounceTimer)
    debounceTimer = setTimeout(() => emit('update:modelValue', value), props.debounceMs)
  } else {
    emit('update:modelValue', value)
  }
}
</script>

<template>
  <div class="relative">
    <label :for="inputId" class="sr-only">{{ placeholder }}</label>
    <input
      :id="inputId"
      :value="local"
      type="search"
      :placeholder="placeholder"
      autocomplete="off"
      class="w-full rounded-lg border border-gray-300 bg-white py-2 pl-10 pr-4 text-sm text-gray-900 shadow-sm placeholder:text-gray-500 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100 dark:placeholder:text-gray-400"
      @input="onInput"
    >
    <span class="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3 text-gray-400">
      <svg class="size-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" aria-hidden="true">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z" />
      </svg>
    </span>
  </div>
</template>
