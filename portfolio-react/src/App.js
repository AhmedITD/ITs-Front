import './App.css';
import Header from './components/Header';
import Projects from './components/Projects';
import Contact from './components/Contact';

function App() {
  return (
    <div className="App">
      <Header />
      
      <main className="container">
        <section className="hero">
          <div className="hero-inner">
            <h2>Hi, I'm Ahmed Imad</h2>
            <p>Frontend developer building accessible, performant web apps with React.</p>
            <a className="btn" href="#projects">See my work</a>
          </div>
          <div className="hero-image">
            <img src="/portfolio/images/photo_2026-01-30_17-13-28.jpg" alt="Ahmed Imad" />
          </div>
        </section>

        <section id="projects" className="section">
          <h3>Projects</h3>
          <Projects />
        </section>

        <section id="contact" className="section">
          <h3>Contact</h3>
          <Contact />
        </section>
      </main>

      <footer className="footer">
        <div>Made with ❤️ using React</div>
        <div><a href="https://react.dev" target="_blank" rel="noreferrer">React Docs</a></div>
      </footer>
    </div>
  );
}

export default App;
