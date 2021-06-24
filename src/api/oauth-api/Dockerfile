FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["oauth-api/oauth-api.csproj", "oauth-api/"]
RUN dotnet restore "oauth-api/oauth-api.csproj"
COPY . .
WORKDIR "/src/oauth-api"
RUN dotnet build "oauth-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "oauth-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "oauth-api.dll"]
