'use client'

import React, { useEffect, useState } from 'react'
import { Criminoso } from '@/types'
import { criminosoService } from '@/services/api'
import { Trash2, Edit, Eye } from 'lucide-react'

interface CriminosoTableProps {
  onEdit?: (criminoso: Criminoso) => void
  onDelete?: (id: number) => void
  isAdmin?: boolean
}

export function CriminosoTable({ onEdit, onDelete, isAdmin }: CriminosoTableProps) {
  const [criminosos, setCriminosos] = useState<Criminoso[]>([])
  const [loading, setLoading] = useState(true)
  const [filtro, setFiltro] = useState('')

  useEffect(() => {
    carregarCriminosos()
  }, [])

  const carregarCriminosos = async () => {
    try {
      setLoading(true)
      const data = await criminosoService.obterTodos()
      setCriminosos(data)
    } catch (error) {
      console.error('Erro ao carregar criminosos:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id: number) => {
    if (!confirm('Tem certeza que deseja deletar?')) return
    try {
      await criminosoService.deletar(id)
      carregarCriminosos()
      onDelete?.(id)
    } catch (error) {
      alert('Erro ao deletar')
    }
  }

  const dataFiltrada = criminosos.filter((c) =>
    c.nomeCompleto.toLowerCase().includes(filtro.toLowerCase()) ||
    c.cpf.includes(filtro)
  )

  if (loading) {
    return <div className="text-center py-8">Carregando...</div>
  }

  return (
    <div className="space-y-4">
      <input
        type="text"
        placeholder="Buscar por nome ou CPF..."
        value={filtro}
        onChange={(e) => setFiltro(e.target.value)}
        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
      />

      <div className="overflow-x-auto">
        <table className="w-full border-collapse">
          <thead>
            <tr className="bg-gray-100 border-b">
              <th className="px-4 py-3 text-left text-sm font-semibold">Nome</th>
              <th className="px-4 py-3 text-left text-sm font-semibold">CPF</th>
              <th className="px-4 py-3 text-left text-sm font-semibold">Status</th>
              <th className="px-4 py-3 text-left text-sm font-semibold">Antecedentes</th>
              <th className="px-4 py-3 text-center text-sm font-semibold">Ações</th>
            </tr>
          </thead>
          <tbody>
            {dataFiltrada.map((criminoso) => (
              <tr
                key={criminoso.id}
                className="border-b hover:bg-gray-50 transition"
              >
                <td className="px-4 py-3 text-sm">{criminoso.nomeCompleto}</td>
                <td className="px-4 py-3 text-sm font-mono">{criminoso.cpf}</td>
                <td className="px-4 py-3 text-sm">
                  <span
                    className={`px-2 py-1 rounded text-xs font-semibold ${
                      criminoso.status === 'Aprovado'
                        ? 'bg-green-100 text-green-800'
                        : criminoso.status === 'Recusado'
                        ? 'bg-red-100 text-red-800'
                        : 'bg-yellow-100 text-yellow-800'
                    }`}
                  >
                    {criminoso.status}
                  </span>
                </td>
                <td className="px-4 py-3 text-sm">
                  <div className="flex gap-1 flex-wrap">
                    {criminoso.antecedentes.map((ant, i) => (
                      <span
                        key={i}
                        className="bg-gray-200 text-gray-800 px-2 py-1 rounded text-xs"
                      >
                        {ant}
                      </span>
                    ))}
                  </div>
                </td>
                <td className="px-4 py-3 text-sm flex justify-center gap-2">
                  <button className="p-1 hover:bg-blue-100 rounded transition">
                    <Eye size={18} className="text-blue-600" />
                  </button>
                  {isAdmin && (
                    <>
                      <button
                        onClick={() => onEdit?.(criminoso)}
                        className="p-1 hover:bg-yellow-100 rounded transition"
                      >
                        <Edit size={18} className="text-yellow-600" />
                      </button>
                      <button
                        onClick={() => handleDelete(criminoso.id)}
                        className="p-1 hover:bg-red-100 rounded transition"
                      >
                        <Trash2 size={18} className="text-red-600" />
                      </button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {dataFiltrada.length === 0 && (
        <div className="text-center py-8 text-gray-500">
          Nenhum criminoso encontrado
        </div>
      )}
    </div>
  )
}
