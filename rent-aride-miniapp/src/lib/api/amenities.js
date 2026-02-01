import { getApi } from './client.js';

export async function getAll() {
  const res = await getApi('/amenities');
  return res?.data ?? res ?? [];
}
