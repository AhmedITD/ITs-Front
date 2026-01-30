<script>
  // example projects - replace with real projects
  export let projects = [
    { id: 1, title: 'Portfolio', desc: 'Personal site built with Svelte', url: '#' },
    { id: 2, title: 'Calculator', desc: 'Small calculator app', url: '/calculator' },
    { id: 3, title: 'Shopping Cart', desc: 'Cart demo app', url: '/cart' }
  ];
</script>

<div class="projects-grid">
  {#each projects as p}
    <article class="project-card">
      <h4>{p.title}</h4>
      <p>{p.desc}</p>
      <div class="meta"><a class="link" href={p.url}>View</a></div>
    </article>
  {/each}
</div>

<style>
  .projects-grid { display:grid; grid-template-columns: repeat(auto-fit,minmax(240px,1fr)); gap:16px }
  .project-card { background: rgba(255,255,255,0.03); padding:16px; border-radius:10px }
  .project-card h4 { margin:0 0 6px 0 }
  .link { color:#667eea; text-decoration:none }
</style>
