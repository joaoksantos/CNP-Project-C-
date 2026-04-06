# ProjetoCNP - Sistema de Gerenciamento de Criminosos

## 🎯 Visão Geral

Sistema completo de gerenciamento de registros de criminosos com interface web moderna, autenticação JWT e diferenciação de permissões.

### Stack Tecnológico

**Backend:**
- ASP.NET Core 9.0
- Entity Framework Core
- MySQL
- JWT Authentication

**Frontend:**
- React 18
- Next.js 14
- TypeScript
- Tailwind CSS

## 📋 Funcionalidades

- ✅ Autenticação JWT com suporte a Admin e Usuário comum
- ✅ CRUD completo de criminosos
- ✅ Busca por nome, CPF e status
- ✅ Dashboard responsivo
- ✅ Painel administrativo
- ✅ Formatação automática de CPF
- ✅ Gestão de antecedentes criminais

## 🚀 Quick Start

### Pré-requisitos

- Docker e Docker Compose (recomendado)
- OU: .NET 9.0 SDK + Node.js 18+

### Opção 1: Docker (Recomendado)

```bash
docker-compose up -d
```

Acesse:
- Frontend: http://localhost:3000
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Opção 2: Execução Local

**Backend:**
```bash
cd ProjetoCNP
dotnet restore
dotnet ef database update
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

## 📚 Estrutura do Projeto

```
ProjetoCNP/
├── Context/                  # Entity Framework DbContext
├── Controllers/              # API endpoints
│   ├── AuthenticationController.cs
│   └── CriminosoController.cs
├── Models/                   # Modelos de dados
│   ├── Criminoso.cs
│   ├── Usuario.cs
│   ├── EnumStatusCriminoso.cs
│   └── EnumAntecedentes.cs
├── Services/                 # Lógica de negócio
│   ├── JwtService.cs
│   ├── AuthenticationService.cs
│   └── CpfFormatter.cs
├── Migrations/               # Migrações do banco
├── appsettings.json         # Configurações
├── Program.cs               # Configuração da aplicação
└── frontend/                # Aplicação React
    ├── src/
    │   ├── app/            # Páginas
    │   ├── components/     # Componentes React
    │   ├── services/       # Serviços API
    │   └── types/          # Tipos TypeScript
    └── package.json
```

## 🔐 Autenticação

### Endpoints

**Login:**
```bash
POST /Autenticacao/login
Content-Type: application/json

{
  "email": "admin@cnp.com",
  "senha": "admin123"
}
```

**Resposta:**
```json
{
  "sucesso": true,
  "mensagem": "Login realizado com sucesso",
  "token": "eyJhbGc...",
  "role": "Admin"
}
```

**Registrar:**
```bash
POST /Autenticacao/registrar
Content-Type: application/json

{
  "email": "novo@cnp.com",
  "senha": "senha123"
}
```

## 📊 Endpoints da API

### Criminosos (Autenticado)

```bash
# Listar todos
GET /Criminoso/ObterTodos

# Obter por ID
GET /Criminoso/{id}

# Buscar por nome
GET /Criminoso/ObterPorNome?nome=João

# Buscar por CPF
GET /Criminoso/ObterPorCPF?cpf=12345678901

# Filtrar por status
GET /Criminoso/ObterPorStatus?status=Aprovado

# Criar (Admin)
POST /Criminoso
Content-Type: application/json

{
  "nomeCompleto": "João Silva",
  "cpf": "12345678901",
  "status": "Pendente",
  "antecedentes": ["Roubo"]
}

# Atualizar (Admin)
PUT /Criminoso/{id}

# Deletar (Admin)
DELETE /Criminoso/{id}
```

## 🎨 Página de Admin

- Criar novo registro de criminoso
- Editar registros existentes
- Deletar registros
- Visualizar todos com filtros
- Gerenciar status e antecedentes

## 🧪 Credenciais Demo

```
Email: admin@cnp.com
Senha: admin123
Role: Admin
```

## 📝 Configuração de Ambiente

### Backend

Edite `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "sua-chave-secreta-aqui",
    "Emissor": "ProjetoCNP",
    "Audiencia": "ProjetoCNPUsers",
    "ExpiracaoMinutos": 60
  },
  "ConnectionStrings": {
    "MinhaConexao": "Server=localhost;Database=bdcnp;Uid=root;Pwd=root"
  }
}
```

### Frontend

Edite `frontend/.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## 🐛 Troubleshooting

**Erro de conexão com banco:**
- Verifique se MySQL está rodando
- Confirme as credenciais em `appsettings.json`

**Token JWT inválido:**
- Verifique se a `SecretKey` está configurada
- Confirme que o token não expirou

**CORS errors:**
- O backend está configurado para aceitar requests de qualquer origem
- Verifique se a URL da API está correta no frontend

## 📞 Suporte

Para mais informações, consulte os READMEs específicos:
- [Backend README](./README_BACKEND.md)
- [Frontend README](./frontend/README.md)

---

**Versão:** 1.0.0  
**Última atualização:** Março 2026
