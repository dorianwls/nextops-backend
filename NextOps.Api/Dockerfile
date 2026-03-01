# Utiliza la imagen oficial de .NET 10 SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copia los archivos de proyecto y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia el resto de los archivos y publica la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Utiliza la imagen runtime de .NET 10 para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expone el puerto por defecto
EXPOSE 80

# Comando para ejecutar la API
ENTRYPOINT ["dotnet", "NextOps.Api.dll"]