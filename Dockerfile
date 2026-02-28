# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Webionic.BloodPressure/Webionic.BloodPressure/Webionic.BloodPressure.csproj", "src/Webionic.BloodPressure/Webionic.BloodPressure/"]
COPY ["src/Webionic.BloodPressure.ServiceDefaults/Webionic.BloodPressure.ServiceDefaults.csproj", "src/Webionic.BloodPressure.ServiceDefaults/"]
COPY ["src/Webionic.BloodPressure/Webionic.BloodPressure.Client/Webionic.BloodPressure.Client.csproj", "src/Webionic.BloodPressure/Webionic.BloodPressure.Client/"]
RUN dotnet restore "./src/Webionic.BloodPressure/Webionic.BloodPressure/Webionic.BloodPressure.csproj"
COPY . .
WORKDIR "/src/src/Webionic.BloodPressure/Webionic.BloodPressure"
RUN dotnet build "./Webionic.BloodPressure.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Webionic.BloodPressure.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /app/data && chown $APP_UID /app/data
USER $APP_UID
VOLUME /app/data
ENTRYPOINT ["dotnet", "Webionic.BloodPressure.dll"]