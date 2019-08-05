FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["ClientBasicCrud.Application/ClientBasicCrud.Application.csproj", "ClientBasicCrud.Application/"]
COPY ["ClientBasicCrud.Model/ClientBasicCrud.Model.csproj", "ClientBasicCrud.Model/"]
RUN dotnet restore "ClientBasicCrud.Application/ClientBasicCrud.Application.csproj"
COPY . .
WORKDIR "/src/ClientBasicCrud.Application"
RUN dotnet build "ClientBasicCrud.Application.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClientBasicCrud.Application.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClientBasicCrud.Application.dll"]
