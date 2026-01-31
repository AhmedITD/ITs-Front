<script>
  import { push, querystring } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { markPaid } from '../lib/stores/session.js';
  import { addPayment } from '../lib/stores/payments.js';
  import { tradePay } from '../lib/qineo/index.js';
  import PaymentForm from '../components/PaymentForm.svelte';

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

  async function handleConfirm() {
    if (!params.sessionId || !(params.amount > 0)) return;
    loading = true;
    try {
      const res = await tradePay({
        token: $auth?.code,
        orderId: params.sessionId,
        amount: params.amount,
        subject: 'Parking - ' + (params.parkName || 'Park'),
      });
      if (res?.success) {
        markPaid(params.sessionId, res.tradeNo);
        addPayment({
          sessionId: params.sessionId,
          amount: params.amount,
          parkName: params.parkName,
          tradeNo: res.tradeNo,
        });
        done = true;
      } else {
        alert('Payment failed. Please try again.');
      }
    } catch (e) {
      alert(e?.message || 'Payment failed');
    } finally {
      loading = false;
    }
  }

  function handleCancel() {
    push('/user/scan');
  }
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else if done}
  <div class="page">
    <h2>Payment Successful</h2>
    <p class="success">{params.amount.toLocaleString()} IQD paid.</p>
    <button class="primary" on:click={() => push('/user')}>Back to Dashboard</button>
    <button on:click={() => push('/user/scan')}>Pay Another</button>
  </div>
{:else if !params.sessionId || !(params.amount > 0)}
  <div class="page">
    <p>Invalid payment data.</p>
    <button class="link-btn" on:click={() => push('/user/scan')}>Scan again</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/user/scan')}>← Back</button>
    <PaymentForm
      amount={params.amount}
      parkName={params.parkName}
      loading={loading}
      onConfirm={handleConfirm}
      onCancel={handleCancel}
    />
  </div>
{/if}

<style>
  .page {
    padding: 1rem;
    max-width: 400px;
    margin: 0 auto;
  }
  .back {
    display: inline-block;
    margin-bottom: 1rem;
    color: #646cff;
  }
  .success {
    color: #4ade80;
    font-size: 1.2rem;
    margin: 1rem 0;
  }
  .page button {
    margin: 0.5rem 0.5rem 0 0;
    padding: 0.6rem 1rem;
    border-radius: 8px;
    cursor: pointer;
    border: 1px solid #444;
    background: #2a2a2a;
    color: #eee;
  }
  .back {
    background: none;
    border: none;
    color: #646cff;
    cursor: pointer;
    font-size: 1rem;
    padding: 0;
    margin-bottom: 1rem;
  }
  .page button.primary {
    background: #646cff;
    border-color: #646cff;
  }
</style>
