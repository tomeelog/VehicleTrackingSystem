using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.Domain.Services
{
    public interface ILocationServices
    {
        Task<List<LocationViewModel>> GetAllLocations();
        Task<ServerResponse> RecordLocation(LocationViewModel entity);
        Task<List<LocationViewModel>> RetriveLocation(LocationViewModel entity);
    }
}
