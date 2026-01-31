import { writable, derived } from 'svelte/store';
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

export const isLoggedIn = derived(auth, ($a) => !!$a?.userInfo);
export const isAdmin = derived(auth, ($a) => $a?.userInfo?.role === 'admin');

export async function login(role = 'user') {
  const res = await getAuthCode({ scopes: role === 'admin' ? ['admin'] : ['userInfo'] });
  let userInfo = res.userInfo;

  if (isSuperQi && res.code && !userInfo) {
    try {
      const data = await authWithSuperQi(res.code);
      userInfo = data.userInfo || data.user || { id: data.id, name: data.name || 'User', role };
    } catch (_) {
      userInfo = { id: res.code?.slice(-8), name: 'User', role };
    }
  }

  if (!userInfo) userInfo = { id: res.code?.slice(-8), name: 'User', role };
  auth.set({ code: res.code, userInfo });
  return userInfo;
}

export function logout() {
  auth.set(null);
}

export function setRole(role) {
  auth.update((a) => {
    if (!a) return a;
    return { ...a, userInfo: { ...a.userInfo, role } };
  });
}
