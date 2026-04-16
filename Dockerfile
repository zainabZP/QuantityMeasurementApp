# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy entire QuantityMeasurementApp folder (contains all projects and solution file)
COPY QuantityMeasurementApp/ .

# Restore dependencies
RUN dotnet restore "QuantityMeasurementApp.slnx"

# Build the project
RUN dotnet build -c Release -o /app/build "QuantityMeasurementApp.slnx"

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish "QuantityMeasurementApi/QuantityMeasurementApi.csproj"

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

# Install ca-certificates for HTTPS
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Expose the port (Render will assign PORT env variable)
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "QuantityMeasurementApi.dll"]
