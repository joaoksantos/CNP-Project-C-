'use client'

import React, { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/services/auth-context'
import { Header } from '@/components/Header'
import { User, Mail, LogOut } from 'lucide-react'

export default function PerfilPage() {
  const { isAuthenticated, loading, logout } = useAuth()
  const router = useRouter()

  useEffect(() => {
    if (!loading && !isAuthenticated) {
      router.push('/')
    }
  }, [isAuthenticated, loading, router])

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Carregando...</div>
  }

  if (!isAuthenticated) {
    return null
  }

  const handleLogout = () => {
    logout()
    router.push('/')
  }

  const email = typeof window !== 'undefined' ? localStorage.getItem('email') || 'user@example.com' : ''

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <main className="max-w-2xl mx-auto px-4 py-8">
        <div className="bg-white rounded-lg shadow p-8">
          <div className="flex justify-center mb-6">
            <div className="w-20 h-20 bg-gradient-to-r from-blue-600 to-purple-600 rounded-full flex items-center justify-center">
              <User size={40} className="text-white" />
            </div>
          </div>

          <h1 className="text-2xl font-bold text-center mb-6">Meu Perfil</h1>

          <div className="space-y-6">
            <div className="flex items-center gap-4 p-4 bg-gray-50 rounded-lg">
              <Mail size={24} className="text-blue-600" />
              <div>
                <p className="text-gray-600 text-sm">Email</p>
                <p className="text-lg font-semibold">{email}</p>
              </div>
            </div>

            <div className="border-t pt-6">
              <h2 className="text-lg font-semibold mb-4">Segurança</h2>
              <button className="w-full px-4 py-2 border border-gray-300 text-gray-700 hover:bg-gray-50 rounded-lg transition mb-4">
                Alterar Senha
              </button>
            </div>

            <div className="border-t pt-6">
              <button
                onClick={handleLogout}
                className="w-full flex items-center justify-center gap-2 bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-lg transition"
              >
                <LogOut size={20} />
                Sair da Conta
              </button>
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
