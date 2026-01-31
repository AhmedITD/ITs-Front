<script>
  import { push } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
  import { sessions } from '../lib/stores/session.js';
  import RoleSwitcher from '../components/RoleSwitcher.svelte';
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else}
  <div class="page">
    <button class="back" on:click={() => push('/admin')}>← Back</button>
    <RoleSwitcher />
    <h2>Session History</h2>

    <ul class="list">
      {#each $sessions as s (s.id)}
        <li class={s.status}>
          <div class="row">
            <span class="amt">{s.amount.toLocaleString()} IQD</span>
            <span class="badge">{s.status}</span>
          </div>
          <div class="meta">{s.parkName} · {new Date(s.createdAt).toLocaleString()}</div>
        </li>
      {/each}
    </ul>
    {#if $sessions.length === 0}
      <p class="empty">No sessions yet.</p>
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
  }
  .list li.paid {
    border-left: 4px solid #4ade80;
  }
  .row {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  .amt {
    font-weight: 600;
  }
  .badge {
    font-size: 0.75rem;
    padding: 0.2rem 0.5rem;
    border-radius: 4px;
    background: #333;
    text-transform: uppercase;
  }
  .list li.paid .badge {
    background: #166534;
  }
  .meta {
    font-size: 0.85rem;
    color: #888;
    margin-top: 0.35rem;
  }
  .empty {
    color: #888;
    text-align: center;
    padding: 2rem;
  }
</style>
