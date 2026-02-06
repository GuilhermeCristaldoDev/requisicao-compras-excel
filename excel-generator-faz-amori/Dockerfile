# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o csproj da subpasta e restaura
COPY excel-generator-faz-amori/*.csproj ./excel-generator-faz-amori/
WORKDIR /src/excel-generator-faz-amori
RUN dotnet restore

# Copia todo o código
COPY excel-generator-faz-amori/. ./
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "excel-generator-faz-amori.dll"]
