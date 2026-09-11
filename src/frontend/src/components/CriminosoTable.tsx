'use client'

import React, { useEffect, useState } from 'react'
import { Criminoso } from '@/types'
import { criminosoService } from '@/services/api'
import { Trash2, Edit, Eye } from 'lucide-react'
import { maskCpf } from '@/utils/maskCpf'

interface CriminosoTableProps {
  criminoso: Criminoso[]
  filtroStatus?: string | null
  onEdit?: (criminoso: Criminoso) => void
  onDelete?: (id: number) => void
  isAdmin?: boolean
}

export function CriminosoTable({ criminoso, filtroStatus, onEdit, onDelete, isAdmin }: CriminosoTableProps) {
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

  const handleStatusChange = async (id: number, status: 'Aprovado' | 'Recusado') => {
    if (!confirm(`Deseja ${status.toLowerCase()} este registro`)) return;
    try {
      await criminosoService.atualizarStatus(id, status)
      carregarCriminosos()
    }catch (error) {
      alert('Erro ao atualizar staus')
    }
  }

  const dataFiltrada = criminosos.filter((c) =>
    c.nomeCompleto.toLowerCase().includes(filtro.toLowerCase()) ||
    c.cpf.includes(filtro)
  )
  .filter((c) =>
    filtroStatus ? c.status === filtroStatus : true
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
              <th className="px-4 py-3 text-left text-sm font-semibold">Situação da Pena</th>
              <th className="px-4 py-3 text-left text-sm font-semibold">Antecedentes</th>
              <th className="px-4 py-3 text-left text-sm font-semibold">Endereço</th>
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
                <td className="px-4 py-3 text-sm font-mono">{isAdmin ? criminoso.cpf : maskCpf(criminoso.cpf)}</td>
                <td className="px-4 py-3 text-sm">
                 {isAdmin && criminoso.status === 'Pendente' ? (
                    <div className="flex gap-2">
                      <button
                        onClick={() => handleStatusChange(criminoso.id, 'Aprovado')}
                        className="px-2 py-1 bg-green-100 text-green-800 rounded text-xs"
                      >
                        Aprovar
                      </button>
                      <button
                        onClick={() => handleStatusChange(criminoso.id, 'Recusado')}
                        className="px-2 py-1 bg-red-100 text-red-800 rounded text-xs"
                      >
                        Recusar
                      </button>
                    </div>
                  ) : (
                    <span className={`px-2 py-1 rounded text-xs font-semibold ${
                      criminoso.status === 'Aprovado'
                        ? 'bg-green-100 text-green-800'
                        : criminoso.status === 'Recusado'
                        ? 'bg-red-100 text-red-800'
                        : 'bg-yellow-100 text-yellow-800'
                    }`}>
                      {criminoso.status}
                    </span>
                  )}
                </td>
                
                <td className="px-4 py-3 text-sm">
                  <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded text-xs">
                    {criminoso.situacaoPena}
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
                <td className="px-4 py-3 text-sm">{criminoso.endereco}</td>
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
