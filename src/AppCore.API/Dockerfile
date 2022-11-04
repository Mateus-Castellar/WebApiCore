FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./AppCore.API/AppCore.API.csproj", "AppCore.API/"]
COPY ["./AppCore.Business/AppCore.Business.csproj", "AppCore.Business/"]
COPY ["./AppCore.Data/AppCore.Data.csproj", "src/AppCore.Data/"]
RUN dotnet restore "AppCore.API/AppCore.API.csproj"
COPY . .
WORKDIR "AppCore.API"
RUN dotnet build "AppCore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppCore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppCore.API.dll"]