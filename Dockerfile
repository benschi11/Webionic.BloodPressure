FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Webionic.BloodPressure/Webionic.BloodPressure/Webionic.BloodPressure.csproj", "src/Webionic.BloodPressure/Webionic.BloodPressure/"]
COPY ["src/Webionic.BloodPressure/Webionic.BloodPressure.Client/Webionic.BloodPressure.Client.csproj", "src/Webionic.BloodPressure/Webionic.BloodPressure.Client/"]
COPY ["src/Webionic.BloodPressure.ServiceDefaults/Webionic.BloodPressure.ServiceDefaults.csproj", "src/Webionic.BloodPressure.ServiceDefaults/"]
RUN dotnet restore "src/Webionic.BloodPressure/Webionic.BloodPressure/Webionic.BloodPressure.csproj"
COPY . .
WORKDIR "/src/src/Webionic.BloodPressure/Webionic.BloodPressure"
RUN dotnet build "Webionic.BloodPressure.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Webionic.BloodPressure.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webionic.BloodPressure.dll"]
