# Use the ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Render provides the PORT environment variable
# EXPOSE 8080 

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file and restore dependencies
# Adjust path assuming Dockerfile is in LinkShortener folder but context is backend folder
COPY ["LinkShortener/LinkShortener.csproj", "LinkShortener/"]
RUN dotnet restore "LinkShortener/LinkShortener.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR "/src/LinkShortener"

# Build the application
RUN dotnet build "LinkShortener.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LinkShortener.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "LinkShortener.dll"]
