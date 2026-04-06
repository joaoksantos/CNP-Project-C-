# ProjetoCNP Frontend

Frontend React + Next.js para o sistema de gerenciamento de criminosos.

## Requisitos

- Node.js 18+
- npm ou yarn

## Instalação

```bash
cd frontend
npm install
```

## Variáveis de Ambiente

Crie um arquivo `.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Executar em Desenvolvimento

```bash
npm run dev
```

A aplicação será aberta em `http://localhost:3000`

## Build para Produção

```bash
npm run build
npm start
```

## Estrutura do Projeto

```
src/
├── app/              # Páginas Next.js
├── components/       # Componentes React reutilizáveis
├── services/         # Serviços (API, Auth)
└── types/           # Tipos TypeScript
```

## Funcionalidades

- ✅ Login/Registro de usuários
- ✅ Dashboard com listagem de criminosos
- ✅ Painel administrativo para CRUD
- ✅ Autenticação JWT
- ✅ Diferenciação entre Admin e Usuário comum
- ✅ Design responsivo com Tailwind CSS

## Credenciais Demo

- Email: `admin@cnp.com`
- Senha: `admin123`
