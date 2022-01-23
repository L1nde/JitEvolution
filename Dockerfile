FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ["JitEvolution.Api/JitEvolution.Api.csproj", "JitEvolution.Api/"]
COPY ["JitEvolution.Api.Dtos/JitEvolution.Api.Dtos.csproj", "JitEvolution.Api.Dtos/"]
COPY ["JitEvolution.BusinessObjects/JitEvolution.BusinessObjects.csproj", "JitEvolution.BusinessObjects/"]
COPY ["JitEvolution.Config/JitEvolution.Config.csproj", "JitEvolution.Config/"]
COPY ["JitEvolution.Data/JitEvolution.Data.csproj", "JitEvolution.Data/"]
COPY ["JitEvolution.Core/JitEvolution.Core.csproj", "JitEvolution.Core/"]
COPY ["JitEvolution.Exceptions/JitEvolution.Exceptions.csproj", "JitEvolution.Exceptions/"]
COPY ["JitEvolution.Neo4J.Data/JitEvolution.Neo4J.Data.csproj", "JitEvolution.Neo4J.Data/"]
COPY ["JitEvolution.Services/JitEvolution.Services.csproj", "JitEvolution.Services/"]
COPY ["JitEvolution.Notifications/JitEvolution.Notifications.csproj", "JitEvolution.Notifications/"]
COPY ["JitEvolution.SignalR/JitEvolution.SignalR.csproj", "JitEvolution.SignalR/"]
RUN dotnet restore "JitEvolution.Api/JitEvolution.Api.csproj"

COPY . .
RUN dotnet publish "JitEvolution.Api/JitEvolution.Api.csproj" -c Release -o out
RUN dotnet tool install -g dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations script --configuration Release --no-build -i -p JitEvolution.Data/ -s JitEvolution.Api/ -o out/databaseMigrations.sql

FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim-arm32v7
ENV ASPNETCORE_URLS=http://*:80
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "JitEvolution.Api.dll"]
