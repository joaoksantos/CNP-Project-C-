'use client'

import React from 'react'
import { LogOut, Menu, X } from 'lucide-react'
import { useAuth } from '@/services/auth-context'
import Link from 'next/link'

export function Header() {
  const { isAuthenticated, logout, isAdmin } = useAuth()
  const [menuOpen, setMenuOpen] = React.useState(false)

  return (
    <header className="bg-gradient-to-r from-blue-600 to-blue-800 text-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
        <div className="flex items-center gap-2">
          <div className="w-8 h-8 bg-white rounded-lg flex items-center justify-center font-bold text-blue-600">
            CNP
          </div>
          <h1 className="text-2xl font-bold">ProjetoCNP</h1>
        </div>

        {isAuthenticated && (
          <nav className="hidden md:flex gap-6 items-center">
            <Link href="/dashboard" className="hover:text-blue-100 transition">
              Dashboard
            </Link>
            {isAdmin && (
              <Link href="/admin" className="hover:text-blue-100 transition">
                Admin
              </Link>
            )}
            <Link href="/perfil" className="hover:text-blue-100 transition">
              Perfil
            </Link>
            <button
              onClick={logout}
              className="flex items-center gap-2 bg-red-500 hover:bg-red-600 px-4 py-2 rounded transition"
            >
              <LogOut size={18} />
              Sair
            </button>
          </nav>
        )}

        {/* Mobile menu */}
        <button
          onClick={() => setMenuOpen(!menuOpen)}
          className="md:hidden"
        >
          {menuOpen ? <X size={24} /> : <Menu size={24} />}
        </button>
      </div>

      {menuOpen && isAuthenticated && (
        <nav className="md:hidden bg-blue-700 px-4 py-4 flex flex-col gap-3">
          <Link href="/dashboard" className="hover:text-blue-100 transition">
            Dashboard
          </Link>
          {isAdmin && (
            <Link href="/admin" className="hover:text-blue-100 transition">
              Admin
            </Link>
          )}
          <Link href="/perfil" className="hover:text-blue-100 transition">
            Perfil
          </Link>
          <button
            onClick={() => {
              logout()
              setMenuOpen(false)
            }}
            className="flex items-center gap-2 bg-red-500 hover:bg-red-600 px-4 py-2 rounded transition justify-start"
          >
            <LogOut size={18} />
            Sair
          </button>
        </nav>
      )}
    </header>
  )
}
