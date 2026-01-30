import { useState } from 'react';
import './Contact.css';

function Contact() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [sent, setSent] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const validateEmail = (e) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(e);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setError('');

    if (!name.trim()) {
      setError('Name is required');
      return;
    }

    if (!validateEmail(email)) {
      setError('Please enter a valid email');
      return;
    }

    if (!message.trim()) {
      setError('Message is required');
      return;
    }

    setLoading(true);

    // Simulate sending
    setTimeout(() => {
      setSent(true);
      setLoading(false);
      setName('');
      setEmail('');
      setMessage('');
      setTimeout(() => setSent(false), 3000);
    }, 1000);
  };

  return (
    <div className="contact-wrapper">
      <div className="contact-info">
        <h3>Get in Touch</h3>
        <p>Have a question or want to collaborate? I'd love to hear from you.</p>

        <div className="info-items">
          <div className="info-item">
            <span className="icon">📧</span>
            <a href="mailto:ahmed.emadtarq@gmail.com">ahmed.emadtarq@gmail.com</a>
          </div>
          <div className="info-item">
            <span className="icon">📍</span>
            <span>Baghdad, Iraq</span>
          </div>
        </div>

        <div class="social-links">
        <a href="https://github.com/AhmedITD" target="_blank" rel="noreferrer" title="GitHub">GitHub</a>
        <a href="https://www.linkedin.com/in/ahmed-imad-55777b250" target="_blank" rel="noreferrer" title="LinkedIn">LinkedIn</a>
        </div>
      </div>

      <form className="contact-form" onSubmit={handleSubmit}>
        <label>
          <span>Name *</span>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Your name"
            required
          />
        </label>

        <label>
          <span>Email *</span>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="your@email.com"
            required
          />
        </label>

        <label>
          <span>Message *</span>
          <textarea
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            rows="5"
            placeholder="Your message..."
            required
          ></textarea>
        </label>

        {error && <div className="error-msg">❌ {error}</div>}

        <button className="btn-submit" type="submit" disabled={loading}>
          {loading ? 'Sending...' : 'Send Message'}
        </button>

        {sent && <div className="success-msg">✅ Message sent successfully! I'll get back to you soon.</div>}
      </form>
    </div>
  );
}

export default Contact;
