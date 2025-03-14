# Step 1: Build the UI project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app/UI

# Copy solution and project files
COPY UI.sln .
COPY UI.csproj .

WORKDIR /app/UI

# Restore dependencies
RUN dotnet restore

# Copy remaining files and build the UI
COPY . .
RUN dotnet publish -c Release -o /app/out

# Step 2: Set up the runtime environment for the UI container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the UI will run on
EXPOSE 5420

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "UI.dll"]