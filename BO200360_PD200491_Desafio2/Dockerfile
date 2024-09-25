FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

# Copia el resto de los archivos y construye la aplicación
COPY . .
RUN dotnet publish -c Release -o out

# Usa la imagen base de .NET ASP.NET para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expone el puerto donde la aplicación escuchará
EXPOSE 8080
ENTRYPOINT ["dotnet", "BO200360_PD200491_Desafio2.dll"]
