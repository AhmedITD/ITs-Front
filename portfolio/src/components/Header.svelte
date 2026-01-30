<script>
  let navOpen = false;
</script>

<header class="site-header">
  <div class="inner">
    <div class="brand">Ahmed Imad</div>
    <nav class="nav" aria-label="Main navigation">
      <a href="#projects">Projects</a>
      <a href="#contact">Contact</a>
      <a href="/" on:click|preventDefault={() => window.scrollTo(0,0)}>Home</a>
    </nav>
    <button class="menu" on:click={() => navOpen = !navOpen} aria-expanded={navOpen} aria-label="Menu">☰</button>
  </div>
  {#if navOpen}
    <div class="mobile-nav">
      <a href="#projects" on:click={() => navOpen = false}>Projects</a>
      <a href="#contact" on:click={() => navOpen = false}>Contact</a>
    </div>
  {/if}
</header>

<style>
  .site-header { background: transparent; padding: 14px 20px; }
  .inner { display:flex; align-items:center; justify-content:space-between; max-width:1000px; margin:0 auto }
  .brand { font-weight:700 }
  .nav a { margin-left:18px; color:#e9e9ff; text-decoration:none }
  .menu { display:none; background:none; border:none; color:#e9e9ff; font-size:1.2rem }
  .mobile-nav { display:flex; flex-direction:column; gap:8px; margin-top:8px }
  @media(max-width:700px) { .nav { display:none } .menu{ display:block } }
</style>
