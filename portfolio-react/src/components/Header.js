import { useState } from 'react';
import './Header.css';

function Header() {
  const [navOpen, setNavOpen] = useState(false);

  return (
    <header className="site-header">
      <div className="inner">
        <div className="brand">Ahmed Imad</div>
        <nav className="nav" aria-label="Main navigation">
          <a href="#projects">Projects</a>
          <a href="#contact">Contact</a>
          <a href="/" onClick={(e) => { e.preventDefault(); window.scrollTo(0, 0); }}>Home</a>
        </nav>
        <button 
          className="menu" 
          onClick={() => setNavOpen(!navOpen)} 
          aria-expanded={navOpen} 
          aria-label="Menu"
        >
          ☰
        </button>
      </div>
      {navOpen && (
        <div className="mobile-nav">
          <a href="#projects" onClick={() => setNavOpen(false)}>Projects</a>
          <a href="#contact" onClick={() => setNavOpen(false)}>Contact</a>
        </div>
      )}
    </header>
  );
}

export default Header;
