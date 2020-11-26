FROM mcr.microsoft.com/dotnet/core/sdk:2.2.105 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.3
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Qnify.dll"]