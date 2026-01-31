<script>
  import { push, querystring } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { tradePay } from '../lib/qineo/index.js';

  $: params = (() => {
    const p = new URLSearchParams($querystring || '');
    return {
      sessionId: p.get('sessionId') || '',
      amount: Number(p.get('amount')) || 0,
      parkName: p.get('parkName') || '',
    };
  })();

  let loading = false;
  let done = false;

  async function handlePay() {
    if (!params.sessionId || !(params.amount > 0)) return;
    loading = true;
    try {
      const res = await tradePay({
        token: $auth?.token || $auth?.code,
        orderId: params.sessionId,
        amount: params.amount,
        subject: 'Parking - ' + (params.parkName || 'Park'),
      });
      if (res?.success) done = true;
      else alert('Payment failed. Please try again.');
    } catch (e) {
      alert(e?.message || 'Payment failed');
    } finally {
      loading = false;
    }
  }

  function goScan() {
    push('/');
  }
</script>

<div class="page">
  {#if done}
    <h2>Payment successful</h2>
    <p class="success">{params.amount.toLocaleString()} IQD paid.</p>
    <button class="primary" on:click={goScan}>Scan again</button>
  {:else if !params.sessionId || !(params.amount > 0)}
    <p>Invalid payment data.</p>
    <button class="link" on:click={goScan}>Scan again</button>
  {:else}
    <h2>Confirm payment</h2>
    {#if params.parkName}
      <p class="park">{params.parkName}</p>
    {/if}
    <p class="amount">{params.amount.toLocaleString()} IQD</p>
    <div class="actions">
      <button disabled={loading} on:click={goScan}>Cancel</button>
      <button class="primary" disabled={loading} on:click={handlePay}>
        {loading ? 'Processing…' : 'Pay'}
      </button>
    </div>
  {/if}
</div>

<style>
  .page {
    padding: 2rem 1rem;
    text-align: center;
    max-width: 400px;
    margin: 0 auto;
  }
  .success {
    color: #4ade80;
    font-size: 1.2rem;
    margin: 1rem 0;
  }
  .park {
    color: #888;
    font-size: 0.9rem;
    margin: 0 0 0.5rem;
  }
  .amount {
    font-size: 1.5rem;
    font-weight: 600;
    margin: 1rem 0;
  }
  .actions {
    display: flex;
    gap: 0.75rem;
    justify-content: center;
    margin-top: 1rem;
  }
  .page button {
    padding: 0.6rem 1.2rem;
    border-radius: 8px;
    cursor: pointer;
    border: 1px solid #444;
    background: #2a2a2a;
    color: #eee;
  }
  .primary {
    background: #646cff;
    border-color: #646cff;
  }
  .link {
    background: none;
    border: none;
    color: #646cff;
  }
  .page button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
</style>
