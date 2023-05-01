FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LanguageService/LanguageService.csproj", "LanguageService/"]
COPY ["Logging/Logging.csproj", "Logging/"]
COPY ["Messages/Messages.csproj", "Messages/"]
RUN dotnet restore "LanguageService/LanguageService.csproj"
COPY . .
WORKDIR "/src/LanguageService"
RUN dotnet build "LanguageService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LanguageService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LanguageService.dll"]
