<script>
  import { onMount } from 'svelte';
  import Router from 'svelte-spa-router';
  import Scan from './routes/Scan.svelte';
  import Pay from './routes/Pay.svelte';
  import { allowSystemSnapshot } from './lib/qineo/index.js';

  onMount(() => allowSystemSnapshot());

  const routes = {
    '/': Scan,
    '/pay': Pay,
  };
</script>

<main>
  <Router {routes} />
</main>

<style>
  main {
    min-height: 100vh;
    padding: 0;
  }
</style>
