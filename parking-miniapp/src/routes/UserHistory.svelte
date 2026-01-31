<script>
  import { push } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { payments } from '../lib/stores/payments.js';
  import RoleSwitcher from '../components/RoleSwitcher.svelte';
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/user')}>← Back</button>
    <RoleSwitcher />
    <h2>Payment History</h2>

    <ul class="list">
      {#each $payments as p (p.id)}
        <li>
          <div class="row">
            <span class="amt">{p.amount.toLocaleString()} IQD</span>
          </div>
          <div class="meta">{p.parkName || 'Parking'} · {new Date(p.createdAt).toLocaleString()}</div>
          {#if p.tradeNo}
            <div class="trade">Ref: {p.tradeNo}</div>
          {/if}
        </li>
      {/each}
    </ul>
    {#if $payments.length === 0}
      <p class="empty">No payments yet.</p>
    {/if}
  </div>
{/if}

<style>
  .page {
    padding: 1rem;
    max-width: 480px;
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
  .list {
    list-style: none;
    padding: 0;
    margin: 0;
  }
  .list li {
    padding: 1rem;
    background: #1e1e1e;
    border-radius: 8px;
    margin-bottom: 0.5rem;
    border-left: 4px solid #4ade80;
  }
  .row {
    display: flex;
    justify-content: space-between;
  }
  .amt {
    font-weight: 600;
  }
  .meta {
    font-size: 0.85rem;
    color: #888;
    margin-top: 0.35rem;
  }
  .trade {
    font-size: 0.75rem;
    color: #666;
    margin-top: 0.25rem;
  }
  .empty {
    color: #888;
    text-align: center;
    padding: 2rem;
  }
</style>
