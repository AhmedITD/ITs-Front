import { writable } from 'svelte/store';
import { getAuthCode, isSuperQi, authWithSuperQi } from '../qineo/index.js';

const AUTH_KEY = 'parking_miniapp_auth';

function loadStored() {
  try {
    const s = localStorage.getItem(AUTH_KEY);
    if (s) return JSON.parse(s);
  } catch (_) {}
  return null;
}

export const auth = writable(loadStored());

auth.subscribe((v) => {
  if (v) localStorage.setItem(AUTH_KEY, JSON.stringify(v));
  else localStorage.removeItem(AUTH_KEY);
});

export async function login() {
  const res = await getAuthCode({ scopes: ['auth_base', 'USER_ID'] });
  let token = null;
  let userInfo = null;

  if (res.code) {
    try {
      const data = await authWithSuperQi(res.code);
      token = data.token || res.code;
      userInfo = data.userInfo || data.user || (data.id && { id: data.id, name: data.name || 'User' });
    } catch (_) {
      token = res.code;
    }
  }
//
  if (!userInfo) userInfo = { id: res.code?.slice(-8), name: 'User' };
  auth.set({ code: res.code, token: token || res.code, userInfo });
  return userInfo;
}
