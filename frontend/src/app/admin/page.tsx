'use client'

import React, { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/services/auth-context'
import { Header } from '@/components/Header'
import { CriminosoForm } from '@/components/CriminosoForm'
import { CriminosoTable } from '@/components/CriminosoTable'
import { Criminoso } from '@/types'
import { Plus } from 'lucide-react'
import { Users, ShieldAlert, ShieldBan, ShieldCheck } from 'lucide-react'
import { criminosoService } from '@/services/api'


export default function AdminPage() {
  const { isAuthenticated, isAdmin, loading } = useAuth()
  const router = useRouter()
  const [showForm, setShowForm] = useState(false)
  const [editingCriminoso, setEditingCriminoso] = useState<Criminoso | undefined>()
  const [refreshKey, setRefreshKey] = useState(0)
  const [loadingData, setLoadingData] = useState(true)
  const [filtroStatus, setFiltroStatus] = useState<string | null>(null)
  const [criminosos, setCriminosos] = useState<Criminoso[]>([])

  useEffect(() => {
    if (!loading && (!isAuthenticated || !isAdmin)) {
      router.push('/')
    }
  }, [isAuthenticated, isAdmin, loading, router])

  useEffect(() => {
    const carregar = async () => {
      try {
        const data = await criminosoService.obterTodos()
        setCriminosos(data)
      }catch (error){
        console.error(error)
      }finally {
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

        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
                  <div className="bg-white rounded-lg shadow p-6 cursor-pointer hover:shadow-lg transition"
                  onClick={() => setFiltroStatus(null)}>
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
                  {/*Campo adicionado manualmente*/}
                  <div className="bg-white rounded-lg shadow p-6 cursor-pointer hover:shadow-lg transition"
                  onClick={()=>setFiltroStatus("Aprovado")}>
                    <div className="flex items-center gap-4">
                      <div className="p-3 bg-green-100 rounded-lg">
                        <ShieldCheck size={24} className="text-green-600" />
                      </div>
                      <div>
                        <p className="text-gray-600 text-sm">Aprovados</p>
                        <p className="text-2xl font-bold">{aprovados}</p>
                      </div>
                    </div>
                  </div>
        
                  <div className="bg-white rounded-lg shadow p-6 cursor-pointer hover:shadow-lg transition"
                  onClick={()=> setFiltroStatus('Pendente')}
                    >
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
        
                  <div className="bg-white rounded-lg shadow p-6 cursor-pointer hover:shadow-lg transition"
                  onClick={()=> setFiltroStatus('Recusado')}>
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

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {showForm && (
            <div className="lg:col-span-1">
              <CriminosoForm
                criminoso={editingCriminoso}
                isAdmin={true}
                onSuccess={handleSuccess}
              />
            </div>
          )}

          <div className={showForm ? 'lg:col-span-2' : 'lg:col-span-3'}>
            <div className="bg-white rounded-lg shadow p-6">
              <h2 className="text-xl font-bold mb-4">Gerenciar Criminosos</h2>
              <CriminosoTable
                key={refreshKey}
                criminoso={criminosos}
                filtroStatus={filtroStatus}
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
