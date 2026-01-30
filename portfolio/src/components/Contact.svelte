<script>
  let name = '';
  let email = '';
  let message = '';
  let sent = false;
  let error = '';
  let loading = false;

  function validateEmail(e) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(e);
  }

  async function submit(e) {
    e.preventDefault();
    error = '';
    
    if (!name.trim()) {
      error = 'Name is required';
      return;
    }
    
    if (!validateEmail(email)) {
      error = 'Please enter a valid email';
      return;
    }
    
    if (!message.trim()) {
      error = 'Message is required';
      return;
    }

    loading = true;
    
    // Simulate sending (replace with real API call)
    setTimeout(() => {
      sent = true;
      loading = false;
      name = '';
      email = '';
      message = '';
      setTimeout(() => { sent = false; }, 3000);
    }, 1000);
  }
</script>

<div class="contact-wrapper">
  <div class="contact-info">
    <h3>Get in Touch</h3>
    <p>Have a question or want to collaborate? I'd love to hear from you.</p>
    
    <div class="info-items">
      <div class="info-item">
        <span class="icon">📧</span>
        <a href="mailto:ahmed.emadtarq@gmail.com">ahmed.emadtarq@gmail.com</a>
      </div>
      <div class="info-item">
        <span class="icon">📍</span>
        <span>Baghdadad, Iraq</span>
      </div>
    </div>

    <div class="social-links">
      <a href="https://github.com/AhmedITD" target="_blank" rel="noreferrer" title="GitHub">GitHub</a>
      <a href="https://www.linkedin.com/in/ahmed-imad-55777b250" target="_blank" rel="noreferrer" title="LinkedIn">LinkedIn</a>
    </div>
  </div>

  <form class="contact-form" on:submit={submit}>
    <label>
      <span>Name *</span>
      <input 
        type="text"
        bind:value={name} 
        placeholder="Your name"
        required 
      />
    </label>

    <label>
      <span>Email *</span>
      <input 
        type="email"
        bind:value={email} 
        placeholder="your@email.com"
        required 
      />
    </label>

    <label>
      <span>Message *</span>
      <textarea 
        bind:value={message} 
        rows="5" 
        placeholder="Your message..."
        required
      ></textarea>
    </label>

    {#if error}
      <div class="error-msg">❌ {error}</div>
    {/if}

    <button class="btn-submit" type="submit" disabled={loading}>
      {loading ? 'Sending...' : 'Send Message'}
    </button>

    {#if sent}
      <div class="success-msg">✅ Message sent successfully! I'll get back to you soon.</div>
    {/if}
  </form>
</div>

<style>
  .contact-wrapper {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 40px;
    margin-top: 20px;
  }

  .contact-info h3 {
    margin-top: 0;
    color: #667eea;
  }

  .contact-info p {
    color: #cfcfe3;
    line-height: 1.6;
  }

  .info-items {
    display: flex;
    flex-direction: column;
    gap: 16px;
    margin: 24px 0;
  }

  .info-item {
    display: flex;
    align-items: center;
    gap: 12px;
    color: #cfcfe3;
  }

  .info-item .icon {
    font-size: 1.4rem;
  }

  .info-item a {
    color: #667eea;
    text-decoration: none;
    transition: color 0.2s;
  }

  .info-item a:hover {
    color: #7a92ff;
  }

  .social-links {
    display: flex;
    gap: 12px;
    margin-top: 20px;
  }

  .social-links a {
    padding: 8px 16px;
    background: rgba(102, 126, 234, 0.1);
    color: #667eea;
    text-decoration: none;
    border-radius: 6px;
    transition: all 0.2s;
    font-size: 0.9rem;
  }

  .social-links a:hover {
    background: rgba(102, 126, 234, 0.2);
    transform: translateY(-2px);
  }

  .contact-form {
    display: flex;
    flex-direction: column;
    gap: 16px;
  }

  .contact-form label {
    display: flex;
    flex-direction: column;
    gap: 6px;
  }

  .contact-form span {
    font-size: 0.9rem;
    color: #cfcfe3;
    font-weight: 500;
  }

  input, textarea {
    padding: 12px;
    border-radius: 8px;
    border: 1px solid rgba(102, 126, 234, 0.2);
    background: rgba(255, 255, 255, 0.03);
    color: inherit;
    font-family: inherit;
    transition: all 0.2s;
  }

  input:focus, textarea:focus {
    outline: none;
    border-color: #667eea;
    background: rgba(255, 255, 255, 0.05);
    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
  }

  .error-msg {
    color: #ff6b6b;
    padding: 10px;
    border-radius: 6px;
    background: rgba(255, 107, 107, 0.1);
    font-size: 0.9rem;
  }

  .success-msg {
    color: #51cf66;
    padding: 10px;
    border-radius: 6px;
    background: rgba(81, 207, 102, 0.1);
    font-size: 0.9rem;
  }

  .btn-submit {
    padding: 12px 20px;
    background: linear-gradient(135deg, #667eea, #764ba2);
    color: white;
    border: none;
    border-radius: 8px;
    font-weight: bold;
    cursor: pointer;
    transition: all 0.2s;
    font-size: 1rem;
  }

  .btn-submit:hover:not(:disabled) {
    transform: translateY(-2px);
    box-shadow: 0 8px 16px rgba(102, 126, 234, 0.3);
  }

  .btn-submit:active:not(:disabled) {
    transform: translateY(0);
  }

  .btn-submit:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }

  @media (max-width: 768px) {
    .contact-wrapper {
      grid-template-columns: 1fr;
      gap: 30px;
    }
  }
</style>
