import type { Metadata } from 'next'
import React from 'react'
import './globals.css'
import { Providers } from './providers'

export const metadata: Metadata = {
  title: 'ProjetoCNP',
  description: 'Dashboard de gerenciamento de criminosos',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="pt-BR">
      <body className="bg-gray-50">
        <Providers>
          {children}
        </Providers>
      </body>
    </html>
  )
}
