#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["VehicleTrackingSystem.API/VehicleTrackingSystem.API.csproj", "VehicleTrackingSystem.API/"]
COPY ["VehicleTrackingSystem.Domain/VehicleTrackingSystem.Domain.csproj", "VehicleTrackingSystem.Domain/"]
COPY ["VehicleTrackingSystem.Utilities/VehicleTrackingSystem.Utilities.csproj", "VehicleTrackingSystem.Utilities/"]
COPY ["VehicleTrackingSystem.DataAccess/VehicleTrackingSystem.DataAccess.csproj", "VehicleTrackingSystem.DataAccess/"]
COPY ["VehicleTrackingSystem.Entities/VehicleTrackingSystem.Entities.csproj", "VehicleTrackingSystem.Entities/"]
COPY ["VehicleTrackingSystem.Security/VehicleTrackingSystem.Security.csproj", "VehicleTrackingSystem.Security/"]
COPY ["VehicleTrackingSystem.CustomObjects/VehicleTrackingSystem.CustomObjects.csproj", "VehicleTrackingSystem.CustomObjects/"]
COPY ["VehicleTrackingSystem.Validation/VehicleTrackingSystem.Validation.csproj", "VehicleTrackingSystem.Validation/"]
RUN dotnet restore "VehicleTrackingSystem.API/VehicleTrackingSystem.API.csproj"
COPY . .
WORKDIR "/src/VehicleTrackingSystem.API"
RUN dotnet build "VehicleTrackingSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleTrackingSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleTrackingSystem.API.dll"]