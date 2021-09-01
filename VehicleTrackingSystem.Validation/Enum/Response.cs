using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VehicleTrackingSystem.Validation.Enum
{
    public enum ResponseEnum
    {
        [Description("Invalid Authorization token")]
        InvalidToken =1,
        [Description("You are not authorized to perform this action")]
        UnAuthorized,
        EmptyToken,
        NotFound,
        [Description("This user already exist")]
        UserExist,
        DeviceExist,
        [Description("Username or password is invalid")]
        InvalidUsernameOrPassword,
        [Description("Username or password cannot be empty")]
        BlankUsernamePassword
    }
}
