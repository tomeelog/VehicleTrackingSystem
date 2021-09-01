Vehicle Tracking System, ASP.NET Core 3.1 WebApi Sample with & Swagger

Dependencies 
-Entity framework core ORM
-Microsoft Bing Location API
-microsoft SQL Server
-Json Web token

*Entities*
-Device
-Location
-User
-userType
-vehicle

To create database schema, go to appsettings.json and change the connection string or both query and command to point to your database.
open package manager console and run the following steps 
1. Add-Migration -Context VehicleTrackingCommandsContext "Migration name"
2. Update-Database -Context VehicleTrackingCommandsContext

run dotnet VehicleTrackingSystem.API.dll or select VehicleTrackingSystem.API as startup project and run with visual studio