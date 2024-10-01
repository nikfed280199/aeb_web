FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ClientHistory.csproj", "./"]
RUN dotnet restore "ClientHistory.csproj"

COPY . .
RUN dotnet build "ClientHistory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientHistory.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ClientHistory.dll"]