<script>
  export let amount = 0;
  export let parkName = '';
  export let loading = false;
  export let onConfirm = () => {};
  export let onCancel = () => {};
</script>

<div class="payment-form">
  <h3>Confirm Payment</h3>
  {#if parkName}
    <p class="park">{parkName}</p>
  {/if}
  <p class="amount">{amount.toLocaleString()} IQD</p>
  <div class="actions">
    <button disabled={loading} on:click={onCancel}>Cancel</button>
    <button class="primary" disabled={loading} on:click={onConfirm}>
      {loading ? 'Processing…' : 'Pay'}
    </button>
  </div>
</div>

<style>
  .payment-form {
    padding: 1.5rem;
    background: #1e1e1e;
    border-radius: 12px;
    text-align: center;
  }
  .payment-form h3 {
    margin: 0 0 0.5rem;
    font-size: 1.1rem;
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
  .actions button {
    padding: 0.6rem 1.2rem;
    border-radius: 8px;
    cursor: pointer;
    border: 1px solid #444;
    background: #2a2a2a;
    color: #eee;
  }
  .actions button.primary {
    background: #646cff;
    border-color: #646cff;
  }
  .actions button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
</style>
