# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies (better caching)
COPY ["PizzaFrontend.csproj", "./"]
RUN dotnet restore PizzaFrontend.csproj

# Copy everything else and publish
COPY . ./
RUN dotnet publish PizzaFrontend.csproj -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8080

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "PizzaFrontend.dll"]