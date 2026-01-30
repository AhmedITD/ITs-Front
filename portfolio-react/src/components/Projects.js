import './Projects.css';

function Projects() {
  const projects = [
    { id: 1, title: 'Portfolio', desc: 'Personal site built with React', url: '#' },
    { id: 2, title: 'Calculator', desc: 'Small calculator app', url: '/calculator' },
    { id: 3, title: 'Shopping Cart', desc: 'Cart demo app', url: '/cart' }
  ];

  return (
    <div className="projects-grid">
      {projects.map(p => (
        <article className="project-card" key={p.id}>
          <h4>{p.title}</h4>
          <p>{p.desc}</p>
          <div className="meta"><a className="link" href={p.url}>View</a></div>
        </article>
      ))}
    </div>
  );
}

export default Projects;
