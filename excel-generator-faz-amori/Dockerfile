# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código (inclui Templates e wwwroot)
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Railway injeta a porta via variável, mas garantimos 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copia build final
COPY --from=build /app/publish .

# Comando de start
ENTRYPOINT ["dotnet", "excel-generator-faz-amori.dll"]
