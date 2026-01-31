<script>
  import { onMount } from 'svelte';
  import { push } from 'svelte-spa-router';
  import { auth, login } from '../lib/stores/auth.js';
  import { scan } from '../lib/qineo/index.js';
  import { decode } from '../lib/qr/decode.js';

  let scanning = true;
  let error = '';

  async function doScan() {
    scanning = true;
    error = '';
    try {
      if (!$auth) await login();
      const raw = await scan();
      const result = decode(raw);
      if (!result.valid) {
        error = result.error || 'Invalid QR code';
        return;
      }
      if (result.type === 'charge') {
        push('/pay?sessionId=' + encodeURIComponent(result.sessionId) +
          '&amount=' + encodeURIComponent(result.amount) +
          '&parkName=' + encodeURIComponent(result.parkName || ''));
      } else {
        error = 'Scan a parking charge QR.';
      }
    } catch (e) {
      error = e?.message || 'Scan failed';
    } finally {
      scanning = false;
    }
  }

  onMount(doScan);
</script>

<div class="page">
  {#if scanning}
    <p class="hint">Opening scanner…</p>
  {:else if error}
    <p class="error">{error}</p>
    <button class="primary" on:click={doScan}>Scan again</button>
  {/if}
</div>

<style>
  .page {
    padding: 2rem 1rem;
    text-align: center;
    max-width: 400px;
    margin: 0 auto;
  }
  .hint {
    color: #888;
  }
  .error {
    color: #f87171;
    margin-bottom: 1rem;
  }
  .primary {
    padding: 0.75rem 1.5rem;
    border-radius: 8px;
    border: none;
    background: #646cff;
    color: white;
    font-size: 1rem;
    cursor: pointer;
  }
</style>
