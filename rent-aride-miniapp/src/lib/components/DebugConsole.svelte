<script lang="ts">
  import { onMount, onDestroy } from 'svelte'

  export let maxLines = 100

  interface LogEntry {
    type: string
    message: string
    timestamp: string
  }

  let logs: LogEntry[] = []
  let isCollapsed = false
  const originalConsole: Record<string, (...args: unknown[]) => void> = {}

  function formatArg(arg: unknown): string {
    if (arg === null) return 'null'
    if (arg === undefined) return 'undefined'
    if (typeof arg === 'object') return JSON.stringify(arg, null, 2)
    return String(arg)
  }

  function addLog(type: string, args: unknown[]): void {
    const timestamp = new Date().toLocaleTimeString()
    const message = args.map(formatArg).join(' ')
    logs = [...logs.slice(-(maxLines - 1)), { type, message, timestamp }]
  }

  function setupInterceptor(): void {
    const methods = ['log', 'warn', 'error', 'info', 'debug'] as const
    methods.forEach((method) => {
      originalConsole[method] = console[method]
      console[method] = (...args: unknown[]) => {
        addLog(method, args)
        originalConsole[method](...args)
      }
    })
  }

  function teardownInterceptor(): void {
    const methods = ['log', 'warn', 'error', 'info', 'debug'] as const
    methods.forEach((method) => {
      if (originalConsole[method]) console[method] = originalConsole[method]
    })
  }

  function clearLogs(): void {
    logs = []
  }

  onMount(() => {
    setupInterceptor()
    return () => teardownInterceptor()
  })

  onDestroy(() => {
    teardownInterceptor()
  })
</script>

<div class="debug-console border-t border-gray-300 bg-stone-900 text-stone-100 text-xs font-mono">
  <div
    class="flex items-center justify-between px-2 py-1.5 bg-stone-800 cursor-pointer select-none"
    role="button"
    tabindex="0"
    on:click={() => (isCollapsed = !isCollapsed)}
    on:keydown={(e) => e.key === 'Enter' && (isCollapsed = !isCollapsed)}
  >
    <span class="font-semibold">Console</span>
    <div class="flex items-center gap-2">
      <button
        type="button"
        class="px-2 py-0.5 rounded bg-stone-700 hover:bg-stone-600"
        on:click|stopPropagation={clearLogs}
      >
        Clear
      </button>
      <span class="text-stone-400">{isCollapsed ? '▼' : '▲'}</span>
    </div>
  </div>
  {#if !isCollapsed}
    <div class="max-h-48 overflow-y-auto overflow-x-auto p-2 space-y-1">
      {#each logs as log}
        <div class="console-entry whitespace-pre-wrap break-words">
          <span class="text-stone-500 mr-2">[{log.timestamp}]</span>
          <span
            class="font-semibold mr-2 {log.type === 'error'
              ? 'text-red-400'
              : log.type === 'warn'
                ? 'text-amber-400'
                : 'text-stone-300'}"
          >[{log.type.toUpperCase()}]</span>
          <span class="text-stone-300">{log.message}</span>
        </div>
      {/each}
      {#if logs.length === 0}
        <p class="text-stone-500">No logs yet.</p>
      {/if}
    </div>
  {/if}
</div>
