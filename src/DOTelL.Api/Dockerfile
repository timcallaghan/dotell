FROM postgres:16.0-alpine3.18 AS base
RUN apk update && apk add --no-cache aspnetcore7-runtime supervisor wget curl
RUN curl -s https://api.github.com/repos/sosedoff/pgweb/releases/latest \
      | grep linux_amd64.zip \
      | grep download \
      | cut -d '"' -f 4 \
      | wget -qi - \
      && unzip pgweb_linux_amd64.zip \
      && rm pgweb_linux_amd64.zip \
      && mv pgweb_linux_amd64 /usr/local/bin/pgweb
WORKDIR /app
EXPOSE 5042
EXPOSE 4317

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DOTelL.Api/DOTelL.Api.csproj", "src/DOTelL.Api/"]
COPY ["src/DOTelL.DataAccess/DOTelL.DataAccess.csproj", "src/DOTelL.DataAccess/"]
COPY ["src/DOTelL.Protos/DOTelL.Protos.csproj", "src/DOTelL.Protos/"]
RUN dotnet restore "src/DOTelL.Api/DOTelL.Api.csproj"
COPY . .
WORKDIR "/src/src/DOTelL.Api"
RUN dotnet build "DOTelL.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DOTelL.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY ["src/DOTelL.Api/supervisord.conf", "/etc/supervisord.conf" ]
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/DOTelL.Api/start-dotell.sh", "/app/start-dotell.sh" ]
COPY ["src/DOTelL.Api/start-pgweb.sh", "/app/start-pgweb.sh" ]
CMD ["/usr/bin/supervisord", "-c", "/etc/supervisord.conf"]