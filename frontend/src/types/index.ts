export interface Criminoso {
  id: number
  nomeCompleto: string
  cpf: string
  status: 'Aprovado' | 'Recusado' | 'Pendente'
  situacaoPena: string
  antecedentes: string[]
  endereco: string
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

//Teste de enum para situação da pena
export type SituacaoPena =
  | 'Desconhecido'
  | 'EmLiberdadecondicional'
  | 'Foragido'
  | 'CumprindoPena'
  | 'EmRegimeSemiAberto'
  | 'EmRegimeFechado'
  | 'Solto'