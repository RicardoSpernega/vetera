# Backend (.NET 10)

## Stack

- ASP.NET Core Web API (.NET 10)
- Clean Architecture
- DDD (entidades e contratos)
- Swagger/OpenAPI
- MemoryCache
- IHttpClientFactory

## Estrutura

- backend/src/Api
- backend/src/Application
- backend/src/Domain
- backend/src/Infrastructure
- backend/tests/Backend.UnitTests
- backend/tests/Backend.IntegrationTests

## Camadas

### Domain
- entidades: Game, Achievement
- sem dependencias de framework

### Application
- DTOs
- contratos de servicos
- orquestracao do caso de uso SearchByNameAsync
- regras de validacao

### Infrastructure
- implementacao do gateway RAWG por HttpClient manual
- cache em memoria
- options de configuracao externa

### Api
- controller fino
- middleware global de excecoes
- swagger e pipeline HTTP

## Endpoint

GET /api/games/search?name=elden ring

Fluxo:
1. SearchFirstGameAsync na RAWG
2. GetGameDetailsAsync por id
3. GetAchievementsAsync por id
4. consolidacao e retorno

## Boas praticas aplicadas

- CancellationToken propagado
- Async/await em todo I/O
- logs basicos para erros externos
- options pattern (RawgOptions)
- response padronizado (ApiResponse)
- tratamento centralizado de excecao

## Configuracao

Arquivo: backend/src/Api/appsettings.Development.json

Campos:
- Rawg:BaseUrl
- Rawg:ApiKey
- Rawg:CacheMinutes

Tambem pode usar variaveis de ambiente:
- Rawg__ApiKey
- Rawg__BaseUrl
- Rawg__CacheMinutes

## Testes backend

- Unit: GameSearchService (validacao e cache)
- Integration: endpoint search com nome vazio retornando 400

Comandos:
- dotnet test backend/tests/Backend.UnitTests/Backend.UnitTests.csproj
- dotnet test backend/tests/Backend.IntegrationTests/Backend.IntegrationTests.csproj
