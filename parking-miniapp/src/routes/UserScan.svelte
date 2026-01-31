<script>
  import { push } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { scan } from '../lib/qineo/index.js';
  import { decode } from '../lib/qr/decode.js';

  let scanning = false;
  let error = '';

  async function doScan() {
    scanning = true;
    error = '';
    try {
      const raw = await scan();
      const result = decode(raw);
      if (!result.valid) {
        error = result.error || 'Invalid QR code';
        return;
      }
      if (result.type === 'charge') {
        push('/user/pay?sessionId=' + encodeURIComponent(result.sessionId) +
          '&amount=' + encodeURIComponent(result.amount) +
          '&parkName=' + encodeURIComponent(result.parkName || ''));
      } else {
        error = 'User-pay QR not supported yet. Scan a charge QR.';
      }
    } catch (e) {
      error = e?.message || 'Scan failed';
    } finally {
      scanning = false;
    }
  }
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/user')}>← Back</button>
    <h2>Scan to Pay</h2>
    <p class="hint">Point camera at the park owner's QR code</p>

    <button
      class="scan-btn primary"
      disabled={scanning}
      on:click={doScan}
    >
      {scanning ? 'Scanning…' : 'Open Scanner'}
    </button>

    {#if error}
      <p class="error">{error}</p>
    {/if}

    <p class="dev-note">In dev: scanner opens a prompt. Enter QR JSON or use default.</p>
  </div>
{/if}

<style>
  .page {
    padding: 1rem;
    text-align: center;
    max-width: 400px;
    margin: 0 auto;
  }
  .back {
    display: inline-block;
    margin-bottom: 1rem;
    color: #646cff;
    background: none;
    border: none;
    cursor: pointer;
    font-size: 1rem;
  }
  .page h2 {
    margin: 0 0 0.5rem;
    font-size: 1.3rem;
  }
  .hint {
    color: #888;
    margin-bottom: 1.5rem;
    font-size: 0.95rem;
  }
  .scan-btn {
    padding: 1rem 2rem;
    font-size: 1.1rem;
    border-radius: 12px;
    border: none;
    cursor: pointer;
    width: 100%;
  }
  .scan-btn.primary {
    background: #646cff;
    color: white;
  }
  .scan-btn:disabled {
    opacity: 0.6;
  }
  .error {
    color: #f87171;
    margin-top: 1rem;
    font-size: 0.9rem;
  }
  .dev-note {
    margin-top: 2rem;
    font-size: 0.75rem;
    color: #666;
  }
</style>
