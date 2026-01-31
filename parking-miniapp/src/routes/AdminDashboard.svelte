<script>
  import { push } from 'svelte-spa-router';
  import { auth, isAdmin } from '../lib/stores/auth.js';
  import { activeSessions } from '../lib/stores/session.js';
  import RoleSwitcher from '../components/RoleSwitcher.svelte';
</script>

{#if !$auth}
  <div class="page">
    <p>Please log in.</p>
    <button class="link-btn" on:click={() => push('/')}>Go to Home</button>
  </div>
{:else}
  <div class="page">
    <RoleSwitcher />
    <h2>Park Owner Dashboard</h2>
    <p class="user">{$auth.userInfo?.name}</p>

    <div class="actions">
      <button class="primary" on:click={() => push('/admin/create')}>
        Create Charge Session
      </button>
      <button on:click={() => push('/admin/history')}>Session History</button>
    </div>

    {#if $activeSessions.length > 0}
      <h3>Active Sessions</h3>
      <ul class="session-list">
        {#each $activeSessions as s (s.id)}
          <li>
            <span>{s.amount.toLocaleString()} IQD</span>
            <button on:click={() => push('/admin/qr?id=' + encodeURIComponent(s.id))}>
              Show QR
            </button>
          </li>
        {/each}
      </ul>
    {/if}
  </div>
{/if}

<style>
  .page {
    padding: 1rem;
    max-width: 480px;
    margin: 0 auto;
  }
  .page h2 {
    margin: 0 0 0.5rem;
    font-size: 1.3rem;
  }
  .user {
    color: #888;
    margin: 0 0 1.5rem;
    font-size: 0.9rem;
  }
  .actions {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
    margin-bottom: 2rem;
  }
  .actions button {
    padding: 0.75rem 1rem;
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
  .session-list {
    list-style: none;
    padding: 0;
    margin: 0;
  }
  .session-list li {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.75rem;
    background: #1e1e1e;
    border-radius: 8px;
    margin-bottom: 0.5rem;
  }
  .session-list button {
    padding: 0.4rem 0.8rem;
    font-size: 0.9rem;
    border-radius: 6px;
    cursor: pointer;
    background: #646cff;
    border: none;
    color: white;
  }
</style>
