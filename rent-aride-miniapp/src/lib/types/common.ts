export interface PaginatedList<T> {
  items: T[]
  totalCount: number
  pageIndex?: number
  totalPages: number
  hasPreviousPage?: boolean
  hasNextPage?: boolean
}
