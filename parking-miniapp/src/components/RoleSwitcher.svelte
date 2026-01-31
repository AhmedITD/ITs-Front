<script>
  import { push, location } from 'svelte-spa-router';

  $: isAdmin = $location?.startsWith('/admin');
  $: isUser = $location?.startsWith('/user');
</script>

<nav class="role-nav">
  <button
    class:active={isAdmin}
    on:click={() => push('/admin')}
  >
    Park Owner
  </button>
  <button
    class:active={isUser}
    on:click={() => push('/user')}
  >
    Car Owner
  </button>
</nav>

<style>
  .role-nav {
    display: flex;
    gap: 0.5rem;
    margin-bottom: 1rem;
  }
  .role-nav button {
    padding: 0.5rem 1rem;
    border-radius: 8px;
    border: 1px solid #444;
    background: #2a2a2a;
    color: #eee;
    cursor: pointer;
  }
  .role-nav button:hover {
    background: #333;
  }
  .role-nav button.active {
    background: #646cff;
    border-color: #646cff;
  }
</style>
