<script>
  import Header from './components/Header.svelte';
  import Projects from './components/Projects.svelte';
  import Contact from './components/Contact.svelte';
</script>

<Header />

<main class="container">
  <section class="hero">
    <div class="hero-inner">
      <h2>Hi, I'm Ahmed Imad</h2>
      <p>Frontend developer building accessible, performant web apps with Svelte.</p>
      <a class="btn" href="#projects">See my work</a>
    </div>
    <div class="hero-image">
      <img src="/images/photo_2026-01-30_17-13-28.jpg" alt="Ahmed Imad" />
    </div>
  </section>

  <section id="projects" class="section">
    <h3>Projects</h3>
    <Projects />
  </section>

  <section id="contact" class="section">
    <h3>Contact</h3>
    <Contact />
  </section>
</main>

<style>
  .container {
    max-width: 1000px;
    margin: 40px auto;
    padding: 0 20px;
  }

  .hero {
    display: grid;
    grid-template-columns: 1fr 180px;
    gap: 20px;
    align-items: center;
    background: linear-gradient(135deg, rgba(255,255,255,0.03), rgba(255,255,255,0.01));
    padding: 24px;
    border-radius: 12px;
  }

  .hero-inner h2 { font-size: 2rem; margin-bottom: 8px; }
  .hero-inner p { color: #cfcfe3; margin-bottom: 12px; }
  .btn {
    display:inline-block; padding:10px 16px; background:#667eea; color:white; border-radius:8px; text-decoration:none;
  }

  .hero-image { font-size: 4rem; display:flex; align-items:center; justify-content:center }
  .hero-image img { width: 180px; height: 180px; border-radius: 12px; object-fit: cover; box-shadow: 0 10px 30px rgba(0,0,0,0.3) }
  .section { margin-top: 32px }
  .footer { text-align:center; margin:60px 0; color:#cfcfe3 }

  @media (max-width:800px) {
    .hero { grid-template-columns: 1fr; text-align:center }
    .hero-image { margin-top:10px }
  }
</style>
