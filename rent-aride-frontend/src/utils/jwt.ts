import type { User } from '@/types/auth'

const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

/** Backend UserRole: Admin = 1, Customer = 2. JWT sends "Admin" | "Customer". */
function roleStringToNumber(role: string | undefined): number {
  if (!role) return 2
  if (role === 'Admin') return 1
  return 0
}

/**
 * Decode JWT payload without verification (API validates the token).
 * Returns user id, email, role from claims: sub, email, role.
 */
export function parseUserFromToken(token: string | null): User | null {
  if (!token || typeof token !== 'string') return null
  const parts = token.split('.')
  if (parts.length !== 3) return null
  try {
    const base64 = parts[1]?.replace(/-/g, '+').replace(/_/g, '/') ?? ''
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=')
    const json = atob(padded)
    const payload = JSON.parse(json) as Record<string, unknown>
    const user: User = {
      id: Number(payload.sub),
      email: payload.email as string,
      role: roleStringToNumber(payload.role as string) as number,
    }
    return user
  } catch {
    return null
  }
}
