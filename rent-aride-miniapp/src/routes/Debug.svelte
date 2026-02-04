<script lang="ts">
  import { push } from 'svelte-spa-router'
  import { getAuthCode } from '../lib/bridge/index.js'
  import { API_BASE } from '../lib/bridge/index.js'
  import { auth } from '../lib/stores/auth.js'
  import DebugConsole from '../lib/components/DebugConsole.svelte'

  let authCode = ''
  let applyStatus: 'idle' | 'loading' | 'success' | 'error' = 'idle'
  let applyMessage = ''

  async function onGetAuthCode(): Promise<void> {
    applyStatus = 'idle'
    applyMessage = ''
    try {
      const res = await getAuthCode({ scopes: ['auth_base', 'USER_ID'] })
      authCode = res?.code ?? ''
    } catch (e) {
      applyMessage = e instanceof Error ? e.message : String(e)
      applyStatus = 'error'
    }
  }

  async function onApplyCode(): Promise<void> {
    if (!authCode) {
      applyMessage = 'Get Auth Code first.'
      applyStatus = 'error'
      return
    }
    applyStatus = 'loading'
    applyMessage = ''
    const base = API_BASE
    try {
      const res = await fetch(`${base}/auth/auth-with-superQi`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ token: authCode }),
      })
      const data = await res.json().catch(() => ({}))
      if (res.ok && (data.success !== false)) {
        applyStatus = 'success'
        applyMessage = 'JWT stored. You can use Pay / Book now.'
        const d = data?.data ?? data
        auth.set({
          code: authCode,
          token: d?.token ?? authCode,
          refreshToken: d?.refreshToken ?? null,
          refreshTokenExpiresAt: d?.refreshTokenExpiresAt ?? null,
          userInfo: d?.userInfo ?? d?.user ?? { id: authCode.slice(-8), name: 'User' },
        })
      } else {
        applyStatus = 'error'
        applyMessage = data?.message ?? data?.errors ? JSON.stringify(data.errors) : `HTTP ${res.status}`
      }
    } catch (e) {
      applyStatus = 'error'
      applyMessage = e instanceof Error ? e.message : String(e)
    }
  }

  function goToBook(): void {
    push('/book')
  }
</script>

<div class="p-4 space-y-4">
  <h1 class="text-xl font-semibold">Debug – Auth steps</h1>
  <p class="text-sm text-gray-600">
    Reference-style flow: Get Auth Code → Apply Code → then use Book to create a payment.
  </p>

  <div class="flex flex-col gap-2">
    <button
      type="button"
      class="rounded-md bg-amber-400 text-stone-900 px-4 py-2 font-medium hover:bg-amber-500"
      on:click={onGetAuthCode}
    >
      Get Auth Code
    </button>
    {#if authCode}
      <p class="text-sm text-gray-600">Code received (…{authCode.slice(-8)})</p>
    {/if}
  </div>

  <div class="flex flex-col gap-2">
    <button
      type="button"
      class="rounded-md bg-blue-500 text-white px-4 py-2 font-medium hover:bg-blue-600 disabled:opacity-50"
      on:click={onApplyCode}
      disabled={!authCode || applyStatus === 'loading'}
    >
      Apply Code
    </button>
    {#if applyStatus === 'loading'}
      <p class="text-sm text-gray-600">Applying…</p>
    {:else if applyStatus === 'success'}
      <p class="text-sm text-green-700">{applyMessage}</p>
    {:else if applyStatus === 'error'}
      <p class="text-sm text-red-700">{applyMessage}</p>
    {/if}
  </div>

  <div class="flex flex-col gap-2">
    <button
      type="button"
      class="rounded-md bg-indigo-600 text-white px-4 py-2 font-medium hover:bg-indigo-700 disabled:opacity-50"
      on:click={goToBook}
      disabled={!$auth?.token}
    >
      Pay (open Book)
    </button>
    {#if !$auth?.token}
      <p class="text-sm text-gray-500">Apply Code first to enable.</p>
    {/if}
  </div>

  <a href="#/" class="inline-block text-sm text-gray-600 hover:text-gray-800">← Back to home</a>

  <div class="mt-6">
    <DebugConsole />
  </div>
</div>
