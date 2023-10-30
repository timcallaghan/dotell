FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tools/TelemetryGenerator/TelemetryGenerator.csproj", "tools/TelemetryGenerator/"]
RUN dotnet restore "tools/TelemetryGenerator/TelemetryGenerator.csproj"
COPY . .
WORKDIR "/src/tools/TelemetryGenerator"
RUN dotnet build "TelemetryGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelemetryGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelemetryGenerator.dll"]
