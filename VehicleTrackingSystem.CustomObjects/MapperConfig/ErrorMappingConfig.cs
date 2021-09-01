using System;
using System.Collections.Generic;
using System.Text;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.CustomObjects.MapperConfig
{
    public class ErrorMappingConfig : IErrorMapper
    {
        public ServerResponse MapToError(ServerResponse response, string errorDetail)
        {
            ServerResponse errorResponse = response;
            errorResponse.RespDesc = errorDetail;
            return errorResponse;

        }

        public UserViewModel MapToError(UserViewModel model, ServerResponse response, string errorDetail)
        {
            UserViewModel request = new UserViewModel
            {
                RespCode = 400,
                RespDesc = errorDetail
            };
            return request;
        }
    }
}
