#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sgd.webapi.autenticador/sgd.webapi.autenticador.csproj", "sgd.webapi.autenticador/"]
RUN dotnet restore "sgd.webapi.autenticador/sgd.webapi.autenticador.csproj"
COPY . .
WORKDIR "/src/sgd.webapi.autenticador"
RUN dotnet build "sgd.webapi.autenticador.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sgd.webapi.autenticador.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sgd.webapi.autenticador.dll"]