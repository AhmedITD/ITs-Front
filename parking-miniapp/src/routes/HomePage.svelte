<script>
  import { push } from 'svelte-spa-router';
  import { auth, login } from '../lib/stores/auth.js';
  import RoleSwitcher from '../components/RoleSwitcher.svelte';
</script>

<div class="home">
  <h1>Parking Payment</h1>
  <p class="sub">Pay for parking via QR scan</p>

  {#if $auth?.userInfo}
    <p class="user">Logged in as {$auth.userInfo.name}</p>
    <RoleSwitcher />
    <div class="actions">
      <button class="primary" on:click={() => push('/admin')}>Park Owner Dashboard</button>
      <button class="primary" on:click={() => push('/user')}>Car Owner Dashboard</button>
    </div>
  {:else}
    <p>Choose your role to continue:</p>
    <div class="actions">
      <button
        class="primary"
        on:click={() => login('admin').then(() => push('/admin'))}
      >
        Park Owner (Admin)
      </button>
      <button
        class="primary"
        on:click={() => login('user').then(() => push('/user'))}
      >
        Car Owner
      </button>
    </div>
  {/if}
</div>

<style>
  .home {
    text-align: center;
    padding: 2rem 1rem;
  }
  .home h1 {
    margin: 0 0 0.5rem;
    font-size: 1.8rem;
  }
  .sub {
    color: #888;
    margin: 0 0 2rem;
  }
  .user {
    margin-bottom: 1rem;
    font-size: 0.95rem;
  }
  .actions {
    display: flex;
    flex-wrap: wrap;
    gap: 0.75rem;
    justify-content: center;
    margin: 1rem 0;
  }
  .actions button {
    padding: 0.75rem 1.25rem;
    border-radius: 8px;
    cursor: pointer;
    font-size: 1rem;
    border: none;
  }
  .primary {
    background: #646cff;
    color: white;
  }
</style>
