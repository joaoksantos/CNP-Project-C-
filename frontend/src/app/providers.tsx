'use client'

import { AuthProvider } from '@/services/auth-context'

export function Providers({ children }: { children: React.ReactNode }) {
  return <AuthProvider>{children}</AuthProvider>
}