# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the solution and project files from QuantityMeasurementApp folder
COPY ["QuantityMeasurementApp/QuantityMeasurementApp.slnx", "."]
COPY ["QuantityMeasurementApp/QM.BusinessLogic/QM.BusinessLogic.csproj", "QM.BusinessLogic/"]
COPY ["QuantityMeasurementApp/QM.Models/QM.Models.csproj", "QM.Models/"]
COPY ["QuantityMeasurementApp/QM.Repository/QM.Repository.csproj", "QM.Repository/"]
COPY ["QuantityMeasurementApp/QuantityMeasurementApi/QuantityMeasurementApi.csproj", "QuantityMeasurementApi/"]

# Update project file to use PostgreSQL
RUN sed -i 's/<PackageReference Include="Microsoft\.EntityFrameworkCore\.SqlServer"/<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL"/g' QM.Repository/QM.Repository.csproj && \
    sed -i 's/<PackageReference Include="Microsoft\.EntityFrameworkCore\.Sqlite"/<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL"/g' QuantityMeasurementApi/QuantityMeasurementApi.csproj

# Restore dependencies
RUN dotnet restore "QuantityMeasurementApp.slnx"

# Copy the entire source from QuantityMeasurementApp folder
COPY QuantityMeasurementApp/ .

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
