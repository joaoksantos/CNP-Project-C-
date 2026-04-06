#!/bin/bash

echo "🚀 ProjetoCNP - Setup Frontend"
echo "================================"

cd frontend

echo "📦 Instalando dependências..."
npm install

echo "🔨 Building aplicação..."
npm run build

echo "✅ Frontend pronto!"
echo ""
echo "Para fazer assim, execute:"
echo "  npm run dev      # Desenvolvimento"
echo "  npm start        # Produção"
echo ""
echo "Ou via Docker Compose na raiz do projeto:"
echo "  docker-compose up"
