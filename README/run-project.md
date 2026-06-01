# Run Project Guide

## Pre-requisitos

### Backend
- .NET SDK 10.0.102 ou superior

### Frontend
- Node.js 22+
- npm 11+
- Angular CLI 17 (opcional global; pode usar npx)

## 1) Setup inicial do repositorio

1. Clonar repositorio
2. Entrar na pasta raiz do projeto
3. Garantir arquivo nuget.config na raiz (ja incluido)

## 2) Configurar API Key RAWG

1. Obter chave em https://rawg.io/apidocs
2. Abrir backend/src/Api/appsettings.Development.json
3. Preencher Rawg.ApiKey

Alternativa via ambiente:
- Windows PowerShell:
  - $env:Rawg__ApiKey="SUA_CHAVE"

## 3) Rodar backend (.NET 10)

1. Restaurar e compilar:
   - dotnet restore backend/src/Vetera.slnx --configfile nuget.config
   - dotnet build backend/src/Vetera.slnx
2. Executar API:
   - dotnet run --project backend/src/Api/Api.csproj
3. URLs esperadas:
   - API: http://localhost:7219
   - Swagger: http://localhost:7219/swagger

Portas:
- HTTP API padrao: 7219

## 4) Rodar frontend (Angular 17)

1. Instalar dependencias:
   - cd frontend
   - npm install
2. Executar em desenvolvimento:
   - npm start
3. URL esperada:
   - http://localhost:4200

Configuracao da API no frontend:
- arquivo: frontend/src/environments/environment.ts
- chave: apiBaseUrl

## 5) Execucao completa do zero

1. Configurar Rawg.ApiKey
2. Iniciar backend
3. Iniciar frontend
4. Abrir browser em http://localhost:4200
5. Pesquisar por "elden ring"
6. Verificar retorno da lista de jogos
7. Clicar em "Ver achievements" em um jogo para abrir a modal
8. Confirmar cards de achievements em layout responsivo

## 6) Testes e qualidade

### Backend
- dotnet test backend/tests/Backend.UnitTests/Backend.UnitTests.csproj
- dotnet test backend/tests/Backend.IntegrationTests/Backend.IntegrationTests.csproj

### Frontend
- npm run lint
- npm test
- npm run build

## 7) Docker (opcional, suportado)

1. Definir variavel RAWG_API_KEY na raiz (ou no ambiente)
2. Executar:
   - docker compose up --build
3. URLs:
   - Frontend: http://localhost:4200
   - Backend: http://localhost:7219

## 8) Troubleshooting

### Erro 401/403 na RAWG
- verificar API key
- confirmar limites da conta na RAWG

### Erro de restore NuGet por feed privado
- usar restore com --configfile nuget.config

### Frontend sem dados
- confirmar backend ativo em localhost:7219
- confirmar apiBaseUrl no environment

### Certificados HTTPS em dev
- o projeto roda em HTTP por padrao neste setup
