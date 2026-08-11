FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DentStarLab.Api/DentStarLab.Api.csproj", "DentStarLab.Api/"]
COPY ["DentStarLab.Application/DentStarLab.Application.csproj", "DentStarLab.Application/"]
COPY ["DentStarLab.Domain/DentStarLab.Domain.csproj", "DentStarLab.Domain/"]
COPY ["DentStarLab.Infrastructure/DentStarLab.Infrastructure.csproj", "DentStarLab.Infrastructure/"]

RUN dotnet restore "DentStarLab.Api/DentStarLab.Api.csproj"

COPY . .

WORKDIR "/src/DentStarLab.Api"
RUN dotnet build "DentStarLab.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DentStarLab.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DentStarLab.Api.dll"]