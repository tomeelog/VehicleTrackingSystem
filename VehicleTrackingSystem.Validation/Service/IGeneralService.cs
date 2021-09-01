using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;


namespace VehicleTrackingSystem.Validation.Service
{
    public interface IGeneralService
    {
        Task<bool> IsAdministrator(string token);
        Task<bool> IsPermittedToAddLocation(string userId, int vehicleId);
        Task<bool> VehicleExists(int vehicleId);
        ServerResponse ValidateLocation(LocationViewModel request);
        bool IsValidToken(string token);
        Task<bool> IsPermittedToEditVehicle(string userId, int vehicleId);
        Task<bool> DeviceExists(Guid number);
        ServerResponse ValidateVehicle(VehicleViewModel request);
        string GetUserFromToken(string token);
        Task<bool> UserExists(string userName);
        ServerResponse ValidateUser(UserViewModel request);
    }
}
