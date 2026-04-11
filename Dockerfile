# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos primero los csproj para la caché
COPY ["YuyoDev.WebAPI/YuyoDev.WebAPI.csproj", "YuyoDev.WebAPI/"]
COPY ["YuyoDev.Application/YuyoDev.Application.csproj", "YuyoDev.Application/"]
COPY ["YuyoDev.Domain/YuyoDev.Domain.csproj", "YuyoDev.Domain/"]
COPY ["YuyoDev.Infrastructure/YuyoDev.Infrastructure.csproj", "YuyoDev.Infrastructure/"]

RUN dotnet restore "YuyoDev.WebAPI/YuyoDev.WebAPI.csproj"

# Copiamos todo el código
COPY . .

# --- LA SOLUCIÓN DEFINITIVA ---
# Destruimos a la fuerza cualquier carpeta bin/obj que Docker haya copiado desde tu host
# y volvemos a restaurar para que Linux genere sus archivos limpios.
RUN find . -type d \( -name "bin" -o -name "obj" \) -prune -exec rm -rf {} +
RUN dotnet restore "YuyoDev.WebAPI/YuyoDev.WebAPI.csproj"

# Ahora sí, publicamos con el entorno inmaculado
WORKDIR "/src/YuyoDev.WebAPI"
RUN dotnet publish "YuyoDev.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "YuyoDev.WebAPI.dll"]