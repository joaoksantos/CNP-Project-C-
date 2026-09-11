@echo off
echo 🚀 ProjetoCNP - Setup Frontend
echo ================================
echo.

cd frontend

echo 📦 Instalando dependências...
call npm install

echo.
echo 🔨 Building aplicação...
call npm run build

echo.
echo ✅ Frontend pronto!
echo.
echo Para executar assim:
echo   npm run dev      # Desenvolvimento
echo   npm start        # Produção
echo.
echo Ou via Docker Compose na raiz do projeto:
echo   docker-compose up
echo.
pause
