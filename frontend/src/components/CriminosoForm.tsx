'use client'

import React, { useState } from 'react'
import { Criminoso } from '@/types'
import { criminosoService } from '@/services/api'

interface CriminosoFormProps {
  criminoso?: Criminoso
  onSuccess?: () => void
}

export function CriminosoForm({ criminoso, onSuccess }: CriminosoFormProps) {
  const [formData, setFormData] = useState({
    nomeCompleto: criminoso?.nomeCompleto || '',
    cpf: criminoso?.cpf || '',
    status: criminoso?.status || 'Pendente',
    antecedentes: criminoso?.antecedentes || [],
  })

  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      setLoading(true)

      if (criminoso?.id) {
        await criminosoService.atualizar(criminoso.id, formData)
      } else {
        await criminosoService.criar(formData)
      }

      alert('Sucesso!')
      onSuccess?.()
    } catch (error) {
      alert('Erro ao salvar')
    } finally {
      setLoading(false)
    }
  }

  const antecedentesOptions = ['Estupro', 'Assédio', 'Roubo', 'Furto']

  return (
    <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow">
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">
          Nome Completo
        </label>
        <input
          type="text"
          value={formData.nomeCompleto}
          onChange={(e) =>
            setFormData({ ...formData, nomeCompleto: e.target.value })
          }
          required
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">
          CPF (apenas números)
        </label>
        <input
          type="text"
          maxLength={11}
          value={formData.cpf}
          onChange={(e) =>
            setFormData({ ...formData, cpf: e.target.value.replace(/\D/g, '') })
          }
          required
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">
          Status
        </label>
        <select
          value={formData.status}
          onChange={(e) =>
            setFormData({ ...formData, status: e.target.value as any })
          }
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="Pendente">Pendente</option>
          <option value="Aprovado">Aprovado</option>
          <option value="Recusado">Recusado</option>
        </select>
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">
          Antecedentes
        </label>
        <div className="space-y-2">
          {antecedentesOptions.map((option) => (
            <label key={option} className="flex items-center gap-2">
              <input
                type="checkbox"
                checked={formData.antecedentes.includes(option)}
                onChange={(e) => {
                  if (e.target.checked) {
                    setFormData({
                      ...formData,
                      antecedentes: [...formData.antecedentes, option],
                    })
                  } else {
                    setFormData({
                      ...formData,
                      antecedentes: formData.antecedentes.filter((a) => a !== option),
                    })
                  }
                }}
                className="w-4 h-4 accent-blue-600"
              />
              <span className="text-sm">{option}</span>
            </label>
          ))}
        </div>
      </div>

      <button
        type="submit"
        disabled={loading}
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded-lg transition disabled:opacity-50"
      >
        {loading ? 'Salvando...' : criminoso ? 'Atualizar' : 'Criar'}
      </button>
    </form>
  )
}
