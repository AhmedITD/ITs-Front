import { writable, derived } from 'svelte/store';

const SESSION_KEY = 'parking_miniapp_sessions';

function loadSessions() {
  try {
    const s = localStorage.getItem(SESSION_KEY);
    if (s) return JSON.parse(s);
  } catch (_) {}
  return [];
}

export const sessions = writable(loadSessions());

sessions.subscribe((v) => {
  localStorage.setItem(SESSION_KEY, JSON.stringify(v));
});

export const activeSessions = derived(sessions, ($s) =>
  $s.filter((x) => x.status === 'pending')
);

export function createSession(amount, notes = '', parkId = 'default', parkName = 'Parking') {
  const id = 'sess-' + Date.now() + '-' + Math.random().toString(36).slice(2, 8);
  const session = {
    id,
    amount: Number(amount),
    notes,
    parkId,
    parkName,
    status: 'pending',
    createdAt: new Date().toISOString(),
  };
  sessions.update((s) => [session, ...s]);
  return session;
}

export function markPaid(sessionId, tradeNo) {
  sessions.update((s) =>
    s.map((x) =>
      x.id === sessionId ? { ...x, status: 'paid', tradeNo, paidAt: new Date().toISOString() } : x
    )
  );
}

export function getSession(sessionId) {
  let found = null;
  sessions.subscribe((s) => {
    found = s.find((x) => x.id === sessionId);
  })();
  return found;
}
