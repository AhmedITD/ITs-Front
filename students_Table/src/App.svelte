<script>
  import StudentsTable from './components/StudentsTable.svelte';
</script>

<div class="app">
  <StudentsTable />
</div>

<style global>
  :global(*) {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  :global(body) {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', sans-serif;
    background: #f5f5f5;
    color: #333;
    min-height: 100vh;
  }

  .app {
    min-height: 100vh;
    padding: 40px 20px;
    background: #f5f5f5;
  }
</style>
