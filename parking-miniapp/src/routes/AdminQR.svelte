<script>
  import { push, querystring } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { sessions } from '../lib/stores/session.js';
  import { encodeCharge } from '../lib/qr/encode.js';
  import QRDisplay from '../components/QRDisplay.svelte';

  $: sessionId = new URLSearchParams($querystring || '').get('id') || '';

  $: session = $sessions.find((s) => s.id === sessionId);
  $: payload = session ? encodeCharge(session) : '';
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else if !session}
  <div class="page">
    <p>Session not found.</p>
    <button class="link-btn" on:click={() => push('/admin')}>Back to Dashboard</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/admin')}>← Back</button>
    <h2>Scan to Pay</h2>
    <p class="park">{session.parkName}</p>
    <p class="amount">{session.amount.toLocaleString()} IQD</p>
    <p class="hint">Car owner scans this QR to pay</p>
    <div class="qr-container">
      <QRDisplay payload={payload} size={220} />
    </div>
    {#if session.status === 'paid'}
      <p class="paid">Paid</p>
    {:else}
      <p class="status">Waiting for payment…</p>
    {/if}
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
    margin: 0 0 0.25rem;
    font-size: 1.3rem;
  }
  .park {
    color: #888;
    font-size: 0.9rem;
    margin: 0 0 0.5rem;
  }
  .amount {
    font-size: 1.5rem;
    font-weight: 600;
    margin: 0.5rem 0;
  }
  .hint {
    color: #888;
    font-size: 0.85rem;
    margin-bottom: 1rem;
  }
  .qr-container {
    margin: 1.5rem 0;
  }
  .status, .paid {
    margin-top: 1rem;
    font-size: 0.9rem;
  }
  .paid {
    color: #4ade80;
  }
</style>
