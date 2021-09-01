using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.Domain.Services
{
    public interface IVehicleServices
    {

        Task<VehicleViewModel> GetUserByVehicle(int vehicleId);
        Task<ServerResponse> AddVehicle(VehicleViewModel entity);
        Task<List<VehicleViewModel>> GetAllVehicles();
        Task<bool> IsVehicleExists(int vehicleId);
        Task<List<VehicleViewModel>> GetAllVehiclesWithDevices();
    }
}
