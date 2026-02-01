import { getApi, postApi } from './client.js';

export async function createInvoice(data) {
  const res = await postApi('/rentals/invoice', data);
  return res?.data ?? res;
}

export async function getMyHistory(pageNumber = 1, pageSize = 10) {
  const res = await getApi('/rentals/my-history', { pageNumber, pageSize });
  return res?.data ?? res ?? { items: [], totalPages: 0, totalCount: 0 };
}
