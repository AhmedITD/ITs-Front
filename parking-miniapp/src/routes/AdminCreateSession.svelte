<script>
  import { push } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { createSession } from '../lib/stores/session.js';

  let amount = 5000;
  let notes = '';
  let parkName = 'Parking Lot';
  let error = '';
  let submitting = false;

  function handleSubmit() {
    error = '';
    const amt = Number(amount);
    if (!(amt > 0)) {
      error = 'Enter a valid amount';
      return;
    }
    submitting = true;
    const session = createSession(amt, notes, 'default', parkName);
    submitting = false;
    push('/admin/qr?id=' + encodeURIComponent(session.id));
  }
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button on:click={() => push('/')}>Go to Home</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/admin')}>← Back</button>
    <h2>Create Charge Session</h2>

    <form on:submit|preventDefault={handleSubmit}>
      <div class="field">
        <label for="amount">Amount (IQD)</label>
        <input
          id="amount"
          type="number"
          min="1"
          bind:value={amount}
          placeholder="5000"
        />
      </div>
      <div class="field">
        <label for="park">Park / Lot Name</label>
        <input id="park" type="text" bind:value={parkName} placeholder="Parking Lot" />
      </div>
      <div class="field">
        <label for="notes">Notes (optional)</label>
        <input id="notes" type="text" bind:value={notes} placeholder="e.g. Hourly rate" />
      </div>
      {#if error}
        <p class="error">{error}</p>
      {/if}
      <button type="submit" class="primary" disabled={submitting}>
        {submitting ? 'Creating…' : 'Create & Show QR'}
      </button>
    </form>
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
    background: none;
    border: none;
    cursor: pointer;
    font-size: 1rem;
  }
  .page h2 {
    margin: 0 0 1rem;
    font-size: 1.3rem;
  }
  .field {
    margin-bottom: 1rem;
  }
  .field label {
    display: block;
    margin-bottom: 0.35rem;
    font-size: 0.9rem;
  }
  .field input {
    width: 100%;
    padding: 0.6rem 0.75rem;
    border-radius: 8px;
    border: 1px solid #444;
    background: #1e1e1e;
    color: #eee;
    box-sizing: border-box;
  }
  .error {
    color: #f87171;
    font-size: 0.9rem;
    margin-bottom: 0.5rem;
  }
  button.primary {
    width: 100%;
    padding: 0.75rem;
    margin-top: 0.5rem;
    border-radius: 8px;
    border: none;
    background: #646cff;
    color: white;
    font-size: 1rem;
    cursor: pointer;
  }
</style>
