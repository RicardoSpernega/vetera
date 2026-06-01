FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/src ./backend/src
COPY nuget.config ./nuget.config
WORKDIR /src/backend/src
RUN dotnet restore Vetera.slnx --configfile /src/nuget.config
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 7219
ENV ASPNETCORE_URLS=http://+:7219
ENTRYPOINT ["dotnet", "Api.dll"]
