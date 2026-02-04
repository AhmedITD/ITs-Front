import type { User } from '@/types/auth'

const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

/** Backend UserRole: Admin = 1, Customer = 2. JWT sends "Admin" | "Customer". */
function roleStringToNumber(role: string | undefined): number {
  if (!role) return 2
  if (role === 'Admin') return 1
  return 2 // Customer
}

/** Backend uses ClaimTypes.Role; JWT may use short "role" or full URI. */
function getRoleFromPayload(payload: Record<string, unknown>): string | undefined {
  const role = payload.role
  if (typeof role === 'string') return role
  const roleFromClaim = payload[ROLE_CLAIM]
  return typeof roleFromClaim === 'string' ? roleFromClaim : undefined
}

/**
 * Decode JWT payload without verification (API validates the token).
 * Returns user id, email, role from claims: sub, email, role (or .NET role claim URI).
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
    const roleStr = getRoleFromPayload(payload)
    const user: User = {
      id: Number(payload.sub),
      email: (payload.email as string) ?? '',
      role: roleStringToNumber(roleStr) as number,
    }
    return user
  } catch {
    return null
  }
}
