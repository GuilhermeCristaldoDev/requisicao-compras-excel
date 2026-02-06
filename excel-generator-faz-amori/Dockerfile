# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia csproj e restaura
COPY *.csproj ./
RUN dotnet restore

# Copia o resto e publica
COPY . ./
RUN dotnet publish -c Release -o /out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Porta que o Railway injeta
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /out .

ENTRYPOINT ["dotnet", "excel-generator-faz-amori.dll"]
