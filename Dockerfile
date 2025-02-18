# Usamos la imagen oficial de .NET SDK para construir y restaurar dependencias
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución y los archivos .csproj en la carpeta correcta
COPY ["Hogar.sln", "./"]
COPY ["Hogar/Hogar.Web.csproj", "Hogar/"]
COPY ["Hogar.Application/Hogar.Application.csproj", "Hogar.Application/"]
COPY ["Hogar.Infraestructure/Hogar.Infraestructure.csproj", "Hogar.Infraestructure/"]

# Restaurar dependencias
RUN dotnet restore "Hogar/Hogar.Web.csproj"

# Copiar el resto de los archivos
COPY . .

# Construir la aplicación
WORKDIR "/src/Hogar"
RUN dotnet build "Hogar.Web.csproj" -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish "Hogar.Web.csproj" -c Release -o /app/publish

# Generamos la imagen final para ejecución con solo el runtime de ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80

# Copiar los archivos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Definir el comando de inicio
ENTRYPOINT ["dotnet", "Hogar.Web.dll"]
