'use client'

import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/services/auth-context'
import { Header } from '@/components/Header'
import { CriminosoForm } from '@/components/CriminosoForm'
import { CriminosoTable } from '@/components/CriminosoTable'
import { Criminoso } from '@/types'
import { Plus } from 'lucide-react'

export default function AdminPage() {
  const { isAuthenticated, isAdmin, loading } = useAuth()
  const router = useRouter()
  const [showForm, setShowForm] = useState(false)
  const [editingCriminoso, setEditingCriminoso] = useState<Criminoso | undefined>()
  const [refreshKey, setRefreshKey] = useState(0)

  useEffect(() => {
    if (!loading && (!isAuthenticated || !isAdmin)) {
      router.push('/')
    }
  }, [isAuthenticated, isAdmin, loading, router])

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Carregando...</div>
  }

  if (!isAuthenticated || !isAdmin) {
    return null
  }

  const handleSuccess = () => {
    setShowForm(false)
    setEditingCriminoso(undefined)
    setRefreshKey((prev) => prev + 1)
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <main className="max-w-7xl mx-auto px-4 py-8">
        <div className="mb-8 flex justify-between items-center">
          <h1 className="text-3xl font-bold">Painel Administrativo</h1>
          <button
            onClick={() => {
              setEditingCriminoso(undefined)
              setShowForm(!showForm)
            }}
            className="flex items-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded-lg transition"
          >
            <Plus size={20} />
            Novo Registro
          </button>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {showForm && (
            <div className="lg:col-span-1">
              <CriminosoForm
                criminoso={editingCriminoso}
                onSuccess={handleSuccess}
              />
            </div>
          )}

          <div className={showForm ? 'lg:col-span-2' : 'lg:col-span-3'}>
            <div className="bg-white rounded-lg shadow p-6">
              <h2 className="text-xl font-bold mb-4">Gerenciar Criminosos</h2>
              <CriminosoTable
                key={refreshKey}
                isAdmin={true}
                onEdit={(criminoso) => {
                  setEditingCriminoso(criminoso)
                  setShowForm(true)
                }}
                onDelete={() => setRefreshKey((prev) => prev + 1)}
              />
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
