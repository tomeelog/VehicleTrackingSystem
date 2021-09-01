using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.Domain.Services
{
    public interface IDeviceServices
    {
        Task<bool> DeviceExists(Guid number);
        Task<List<DeviceViewModel>> GetAllDevices();
        Task AddDevice(DeviceViewModel entity);
    }
}
