using System;
using System.Collections.Generic;
using System.Text;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.CustomObjects.MapperConfig
{
    public interface IErrorMapper
    {
        ServerResponse MapToError(ServerResponse response, string errorDetail);
        UserViewModel MapToError(UserViewModel model, ServerResponse response, string errorDetail);
    }
}
