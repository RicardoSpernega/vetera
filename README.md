# Vetera RAWG Challenge

Projeto fullstack para desafio tecnico consumindo a API RAWG com backend ASP.NET Core Web API em .NET 10 e frontend Angular 17.

## Visao geral

A aplicacao permite pesquisar um jogo pelo nome e visualizar:
- imagem de capa do jogo
- nome e descricao
- lista de achievements (imagem, titulo e descricao)

Regra central do desafio respeitada:
- o frontend nao acessa a RAWG diretamente
- apenas a API .NET realiza chamadas HTTP para a RAWG

## Tecnologias

- Backend: ASP.NET Core Web API, .NET 10, IHttpClientFactory, MemoryCache, Swagger
- Frontend: Angular 17 standalone, Angular Material, RxJS, Signals
- Qualidade: xUnit, Karma/Jasmine, ESLint, Prettier
- Containers: Docker e Docker Compose

## Arquitetura escolhida

- Clean Architecture com camadas Api, Application, Domain e Infrastructure
- DDD leve com entidades de dominio Game e Achievement
- Services de aplicacao para orquestrar o caso de uso
- Gateway de infraestrutura para encapsular integracao RAWG

Detalhes completos:
- [README/architecture.md](README/architecture.md)
- [README/backend.md](README/backend.md)
- [README/frontend.md](README/frontend.md)
- [README/run-project.md](README/run-project.md)

## Estrutura de pastas

- /README
	- backend.md
	- frontend.md
	- architecture.md
	- run-project.md
- /backend
- /frontend

## Endpoint principal

GET /api/games/search?name=elden ring

Resposta:

{
	"success": true,
	"data": {
		"name": "Elden Ring",
		"description": "...",
		"backgroundImage": "https://...",
		"achievements": [
			{
				"title": "...",
				"description": "...",
				"image": "https://..."
			}
		]
	},
	"error": null
}

## Como executar

Consulte o passo a passo detalhado em [README/run-project.md](README/run-project.md).

Resumo rapido:
1. Configurar chave RAWG no backend.
2. Rodar API .NET 10 em localhost:7219.
3. Rodar Angular 17 em localhost:4200.

## Configuracao da API Key RAWG

- Criar chave em https://rawg.io/apidocs
- Definir em backend/src/Api/appsettings.Development.json em Rawg:ApiKey
- Alternativa: variavel de ambiente Rawg__ApiKey

## Decisoes tecnicas

- IHttpClientFactory para conexoes HTTP reutilizaveis e testaveis
- Options Pattern para configuracao fortemente tipada da RAWG
- Global Exception Middleware para resposta de erro consistente
- MemoryCache para reduzir latencia e chamadas repetidas
- Debounce no frontend para evitar excesso de requests

## Melhorias futuras

- Retry com Polly para falhas transientes
- Circuit breaker para robustez em indisponibilidade externa
- Observabilidade com OpenTelemetry
- Testes E2E com Playwright
- Internationalization no frontend

## Screenshots (placeholders)

- [ ] Home com busca
- [ ] Resultado de jogo encontrado
- [ ] Estado de erro amigavel
- [ ] Estado vazio/nao encontrado

## Convencoes

- Nomes em ingles para codigo e contratos
- Async/await em todo fluxo de IO
- DTOs separados das entidades de dominio
- Controllers sem regra de negocio
