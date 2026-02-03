import './app.css'
import App from './App.svelte'

// So the inline adapter in index.html uses the correct API URL
if (typeof window !== 'undefined') {
  window.__RENTARIDE_API_BASE__ = import.meta.env.VITE_API_URL
}

const app = new App({
  target: document.getElementById('app')
})

export default app
