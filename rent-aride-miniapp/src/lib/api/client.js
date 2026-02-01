import { get } from 'svelte/store';
import { auth } from '../stores/auth.js';

const API_BASE = import.meta.env.VITE_API_URL || 'http://localhost:5022';

function buildUrl(path) {
  const base = API_BASE.replace(/\/$/, '');
  const p = path.startsWith('/') ? path : '/' + path;
  return base + p;
}

async function request(method, path, body = null, params = {}) {
  const url = new URL(buildUrl(path));
  Object.entries(params).forEach(([k, v]) => {
    if (v != null && v !== '') url.searchParams.set(k, String(v));
  });

  const headers = {
    'Content-Type': 'application/json',
    Accept: 'application/json',
  };
  const a = get(auth);
  if (a?.token) {
    headers['Authorization'] = `Bearer ${a.token}`;
  }

  const config = { method, headers };
  if (body && (method === 'POST' || method === 'PUT')) {
    config.body = JSON.stringify(body);
  }

  const res = await fetch(url.toString(), config);
  const data = await res.json().catch(() => ({}));

  if (!res.ok) {
    const msg = data?.message || (data?.errors ? JSON.stringify(data.errors) : null) || `Request failed (${res.status})`;
    throw new Error(msg);
  }

  if (data.success === false && data.message) {
    throw new Error(data.message);
  }

  return data;
}

export async function getApi(path, params = {}) {
  return request('GET', path, null, params);
}

export async function postApi(path, body) {
  return request('POST', path, body);
}
