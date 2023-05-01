FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GreetingService/GreetingService.csproj", "GreetingService/"]
RUN dotnet restore "GreetingService/GreetingService.csproj"
COPY . .
WORKDIR "/src/GreetingService"
RUN dotnet build "GreetingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreetingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreetingService.dll"]
