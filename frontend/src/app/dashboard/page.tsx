'use client'

import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/services/auth-context'
import { Header } from '@/components/Header'
import { CriminosoTable } from '@/components/CriminosoTable'
import { Users, Shield, CheckCircle, ShieldAlert, ShieldBan } from 'lucide-react'
import { criminosoService } from '@/services/api'
import { Criminoso } from '@/types'

export default function DashboardPage() {
  const { isAuthenticated, loading } = useAuth()
  const router = useRouter()
  const [criminosos, setCriminosos] = useState<Criminoso[]>([])
  const [loadingData, setLoadingData] = useState(true)

  useEffect(() => {
    if (!loading && !isAuthenticated) {
      router.push('/')
    }
  }, [isAuthenticated, loading, router])

  useEffect(() => {
    const carregar = async () => {
      try {
        const data = await criminosoService.obterTodos()
        setCriminosos(data)
      } catch (error) {
        console.error(error)
      } finally {
        setLoadingData(false)
      }
    }
    carregar()
  }, [])

  const total = criminosos.length
  const aprovados = criminosos.filter(c => c.status === 'Aprovado').length
  const pendentes = criminosos.filter(c => c.status === 'Pendente').length
  const recusados = criminosos.filter(c => c.status === 'Recusado').length

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Carregando...</div>
  }

  if (!isAuthenticated) {
    return null
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <main className="max-w-7xl mx-auto px-4 py-8">
        {/* Stats */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center gap-4">
              <div className="p-3 bg-blue-100 rounded-lg">
                <Users size={24} className="text-blue-600" />
              </div>
              <div>
                <p className="text-gray-600 text-sm">Total de Registros</p>
                <p className="text-2xl font-bold">{total}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center gap-4">
              <div className="p-3 bg-orange-100 rounded-lg">
                <ShieldAlert size={24} className="text-orange-600" />
              </div>
              <div>
                <p className="text-gray-600 text-sm">Pendentes</p>
                <p className="text-2xl font-bold">{pendentes}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center gap-4">
              <div className="p-3 bg-red-100 rounded-lg">
                <ShieldBan size={24} className="text-orange-600" />
              </div>
              <div>
                <p className="text-gray-600 text-sm">Recusados</p>
                <p className="text-2xl font-bold">{recusados}</p>
              </div>
            </div>
          </div>
        </div>

        {/* Table */}
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-xl font-bold mb-4">Registros de Criminosos</h2>
          <CriminosoTable />
        </div>
      </main>
    </div>
  )
}
