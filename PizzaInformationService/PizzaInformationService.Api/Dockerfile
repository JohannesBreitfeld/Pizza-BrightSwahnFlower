FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["PizzaInformationService.Api/PizzaInformationService.Api.csproj", "PizzaInformationService.Api/"]
COPY ["PizzaInformationService.Application/PizzaInformationService.Application.csproj", "PizzaInformationService.Application/"]
COPY ["PizzaInformationService.Infrastructure/PizzaInformationService.Infrastructure.csproj", "PizzaInformationService.Infrastructure/"]
COPY ["PizzaInformationService.Domain/PizzaInformationService.Domain.csproj", "PizzaInformationService.Domain/"]

RUN dotnet restore "PizzaInformationService.Api/PizzaInformationService.Api.csproj"

# Copy source code
COPY . .

WORKDIR "/src/PizzaInformationService.Api"
RUN dotnet build "PizzaInformationService.Api.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/PizzaInformationService.Api"
RUN dotnet publish "PizzaInformationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PizzaInformationService.Api.dll"]
