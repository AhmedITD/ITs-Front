<script>
  import { push } from 'svelte-spa-router';
  import { auth } from '../lib/stores/auth.js';
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
    <h2>Car Owner Dashboard</h2>
    <p class="user">{$auth.userInfo?.name}</p>

    <div class="actions">
      <button class="primary" on:click={() => push('/user/scan')}>
        Scan QR to Pay
      </button>
      <button on:click={() => push('/user/history')}>Payment History</button>
    </div>
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
</style>
