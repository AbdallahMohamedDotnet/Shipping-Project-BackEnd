# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file and project files
COPY *.sln .
COPY Ui/*.csproj ./Ui/
COPY BL/*.csproj ./BL/
COPY DAL/*.csproj ./DAL/
COPY Domains/*.csproj ./Domains/
			
# Restore dependencies
RUN dotnet restore Ui/Ui.csproj

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/Ui
RUN dotnet build "Ui.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Ui.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9 runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install SQL Server tools for potential debugging (optional)
RUN apt-get update && apt-get install -y curl

# Copy the published application
COPY --from=publish /app/publish .

# Expose the port the app runs on
EXPOSE 8080
EXPOSE 8081

# Set the entry point
ENTRYPOINT ["dotnet", "Ui.dll"]