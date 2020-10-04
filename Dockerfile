FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-build-env

RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_12.x | bash \
    && apt-get install nodejs -yq
WORKDIR /
COPY . .
RUN dotnet restore
WORKDIR /src/WebUI
RUN dotnet build "WebUI.csproj" -c Release -o /app
WORKDIR /src/WebUI/ClientApp
RUN npm install
RUN npm run build

FROM dotnet-build-env AS publish
WORKDIR /src/WebUI
RUN dotnet publish "WebUI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebUI.dll", "--server.urls", "http://0.0.0.0:5000"]