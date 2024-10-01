FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ClientViewer.csproj", "./"]
RUN dotnet restore "ClientViewer.csproj"

COPY . .
RUN dotnet build "ClientViewer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientViewer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ClientViewer.dll"]