# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for efficient layer caching during restore
COPY ["DeliveryTracking.API/DeliveryTracking.API.csproj", "DeliveryTracking.API/"]
COPY ["DeliveryTracking.Application/DeliveryTracking.Application.csproj", "DeliveryTracking.Application/"]
COPY ["DeliveryTracking.Infrastructure/DeliveryTracking.Infrastructure.csproj", "DeliveryTracking.Infrastructure/"]
COPY ["DeliveryTracking.Persistence/DeliveryTracking.Persistence.csproj", "DeliveryTracking.Persistence/"]
COPY ["DeliveryTracking.Domain/DeliveryTracking.Domain.csproj", "DeliveryTracking.Domain/"]
COPY ["DeliveryTracking.Shared/DeliveryTracking.Shared.csproj", "DeliveryTracking.Shared/"]

# Restore dependencies for the API project (pulls in referenced projects)
RUN dotnet restore "DeliveryTracking.API/DeliveryTracking.API.csproj"

# Copy the rest of the source code
COPY . .

# Publish the API to /app/publish
WORKDIR "/src/DeliveryTracking.API"
RUN dotnet publish "DeliveryTracking.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Render injects the PORT environment variable. Bind Kestrel to it (default 8080 locally).
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet DeliveryTracking.API.dll"]
