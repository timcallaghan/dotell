FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 4317

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DOTelL.Api/DOTelL.Api.csproj", "DOTelL.Api/"]
RUN dotnet restore "DOTelL.Api/DOTelL.Api.csproj"
COPY . .
WORKDIR "/src/DOTelL.Api"
RUN dotnet build "DOTelL.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DOTelL.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DOTelL.Api.dll"]
