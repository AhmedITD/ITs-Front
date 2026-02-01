import { getApi } from './client.js';

export async function getTypes() {
  const res = await getApi('/vehicles/types');
  return res?.data ?? res ?? [];
}

export async function browse(params = {}) {
  const res = await getApi('/vehicles', params);
  return res?.data ?? res ?? { items: [], totalPages: 0, totalCount: 0 };
}
