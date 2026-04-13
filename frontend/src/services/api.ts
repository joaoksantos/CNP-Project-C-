import axios from 'axios'

const API_URL = /*process.env.NEXT_PUBLIC_API_URL ||*/ 'http://localhost:5123'

const api = axios.create({
  baseURL: API_URL,
  timeout: 10000,
})

// Interceptor para adicionar token JWT
api.interceptors.request.use((config) => {
  const token = typeof window !== 'undefined' ? localStorage.getItem('token') : null
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export const authService = {
  login: async (email: string, senha: string) => {
    const response = await api.post('/Authentication/login', { email, senha })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token)
      localStorage.setItem('role', response.data.role)
      localStorage.setItem('email', email)
    }
    return response.data
  },

  register: async (email: string, senha: string) => {
    const response = await api.post('/Authentication/registrar', { email, senha })
    return response.data
  },

  logout: () => {
    localStorage.removeItem('token')
    localStorage.removeItem('role')
  },

  getToken: () => localStorage.getItem('token'),
  getRole: () => localStorage.getItem('role'),
  getEmail: () => localStorage.getItem('email'),
}

export const criminosoService = {
  obterTodos: async () => {
    const response = await api.get('/Criminoso/ObterTodos')
    return response.data
  },

  obterPorId: async (id: number) => {
    const response = await api.get(`/Criminoso/${id}`)
    return response.data
  },

  obterPorNome: async (nome: string) => {
    const response = await api.get('/Criminoso/ObterPorNome', { params: { nome } })
    return response.data
  },

  obterPorCPF: async (cpf: string) => {
    const response = await api.get('/Criminoso/ObterPorCPF', { params: { cpf } })
    return response.data
  },

  obterPorStatus: async (status: string) => {
    const response = await api.get('/Criminoso/ObterPorStatus', { params: { status } })
    return response.data
  },

  criar: async (data: any) => {
    const response = await api.post('/Criminoso', data)
    return response.data
  },

  atualizar: async (id: number, data: any) => {
    const response = await api.put(`/Criminoso/${id}`, data)
    return response.data
  },

  deletar: async (id: number) => {
    const response = await api.delete(`/Criminoso/${id}`)
    return response.data
  },

  atualizarStatus: async (id: number, status: string) => {
    const response = await api.patch(`/Criminoso/${id}/status`, { status })
    return response.data
  }
}

export default api
