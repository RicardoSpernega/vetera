# Frontend (Angular 17)

## Stack

- Angular 17 standalone components
- Angular Material
- RxJS + debounce
- Signals para estado local simples
- ESLint + Prettier

## Estrutura

- frontend/src/app/core
- frontend/src/app/shared
- frontend/src/app/features

## Funcionalidades

- campo de pesquisa por nome do jogo
- botao de busca
- debounce automatico no input
- exibicao de jogo (imagem, nome, descricao)
- botao "Ver achievements" em cada jogo
- modal de achievements com cards lado a lado
- loading skeleton
- erro amigavel
- estado de nao encontrado
- fallback de imagens

## Integracao

Somente com backend .NET:
- endpoint consumido: /api/games/search
- sem chamadas diretas para RAWG no browser

## Arquitetura frontend

- core/services: servicos HTTP e loading global
- core/interceptors: loading interceptor e error interceptor
- shared/components: componentes reutilizaveis
- shared/directives: diretiva de fallback para img
- features/game-search: pagina principal da feature

## Responsividade e UX

- layout com breakpoints para mobile/desktop
- cards adaptativos com CSS grid
- modal responsiva para achievements (desktop e mobile)
- feedback visual de carregamento e erro

## Qualidade

Comandos:
- npm run lint
- npm test
- npm run build
