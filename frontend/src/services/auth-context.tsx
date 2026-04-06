import React, { createContext, useContext, useEffect, useState } from 'react'
import { authService} from './api'

interface AuthContextType {
  isAuthenticated: boolean
  isAdmin: boolean
  loading: boolean
  login: (email: string, senha: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [isAdmin, setIsAdmin] = useState(false)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const token = authService.getToken()
    const role = authService.getRole()
    setIsAuthenticated(!!token)
    setIsAdmin(role === 'Admin')
    setLoading(false)
  }, [])

  const login = async (email: string, senha: string) => {
    try {
      const response = await authService.login(email, senha)
      if (response.sucesso) {
        setIsAuthenticated(true)
        setIsAdmin(response.role === 'Admin')
        localStorage.setItem('email', email)
      }
    } catch (error) {
      throw error
    }
  }

  const logout = () => {
    authService.logout()
    setIsAuthenticated(false)
    setIsAdmin(false)
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, isAdmin, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth deve ser usado dentro de AuthProvider')
  }
  return context
}
