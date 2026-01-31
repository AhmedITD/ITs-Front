<script>
  import Router from 'svelte-spa-router';
  import HomePage from './routes/HomePage.svelte';
  import AdminDashboard from './routes/AdminDashboard.svelte';
  import AdminCreateSession from './routes/AdminCreateSession.svelte';
  import AdminQR from './routes/AdminQR.svelte';
  import AdminHistory from './routes/AdminHistory.svelte';
  import UserDashboard from './routes/UserDashboard.svelte';
  import UserScan from './routes/UserScan.svelte';
  import UserPay from './routes/UserPay.svelte';
  import UserHistory from './routes/UserHistory.svelte';

  const routes = {
    '/': HomePage,
    '/admin': AdminDashboard,
    '/admin/create': AdminCreateSession,
    '/admin/qr': AdminQR,
    '/admin/history': AdminHistory,
    '/user': UserDashboard,
    '/user/scan': UserScan,
    '/user/pay': UserPay,
    '/user/history': UserHistory,
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
