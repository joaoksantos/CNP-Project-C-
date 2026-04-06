export interface Criminoso {
  id: number
  nomeCompleto: string
  cpf: string
  status: 'Aprovado' | 'Recusado' | 'Pendente'
  antecedentes: string[]
}

export interface LoginRequest {
  email: string
  senha: string
}

export interface LoginResponse {
  sucesso: boolean
  mensagem: string
  token: string
  role: string
}

export interface Usuario {
  id: number
  email: string
  role: string
  ativo: boolean
}
