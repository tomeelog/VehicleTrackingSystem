using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTrackingSystem.API
{
    public static class Endpoints
    {
        public const string Login = "Login";
        public const string Registration = "Register";
        public const string RegisterVehicle = "RegisterVehicle";
        public const string GetAllVehicles = "GetAllVehicles";
        public const string GetAllVehiclesWithDevices = "GetAllVehiclesWithDevices";
        public const string RecordLocation = "RecordLocation";
        public const string GetCurrentLocation = "GetCurrentLocation";
        public const string GetAllLocations = "GetAllLocations";
    }

    public static class ServiceConsumesType
    {
        public const string Json = "application/json";
        public const string Xml = "application/xml";
    }

    public static class AllowAccess
    {
        public const string Admin = "Admin";
    }
}
