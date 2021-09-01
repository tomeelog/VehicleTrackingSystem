using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.CustomObjects.Settings;
using VehicleTrackingSystem.Domain.Services;
using VehicleTrackingSystem.Security.Handlers;
using VehicleTrackingSystem.Validation.Enum;

namespace VehicleTrackingSystem.Validation.Service
{
    public class GeneralService : IGeneralService
    {
        private readonly IUserServices _userServices;
        private readonly IJwtTokenHandler _tokenHandler;
        private readonly IDeviceServices _deviceServices;
        private readonly IVehicleServices _vehicleServices;
        private readonly AppSettings _appSettings;
        private readonly ILogger<GeneralService> _logger;

        public GeneralService(IUserServices userServices, IJwtTokenHandler tokenHandler, IDeviceServices deviceServices, IVehicleServices vehicleServices, 
            AppSettings appSettings, ILogger<GeneralService> logger)
        {
            _userServices = userServices;
            _tokenHandler = tokenHandler;
            _deviceServices = deviceServices;
            _vehicleServices = vehicleServices;
            _appSettings = appSettings;
           _logger = logger;
        }
        public async Task<bool> IsAdministrator(string token)
        {
            var result = _tokenHandler.VerifyJwtSecurityToken(token);
            if (string.IsNullOrEmpty(result)) return false;
            long userTypeId = await _userServices.GetUserTypeByUser(Convert.ToInt32(result));
            if (userTypeId == (long)UserTypes.Admin) return true;
            return false;
        }
        public async Task<bool> UserExists(string userName)
        {
            return await _userServices.UserExists(userName);
        }

        public string GetUserFromToken(string token)
        {
            var result = _tokenHandler.VerifyJwtSecurityToken(token);
            return result;
        }

        public async Task<bool> VehicleExists(int vehicleId)
        {
            return await _vehicleServices.IsVehicleExists(vehicleId);
        }
        public bool IsValidToken(string token)
        {
            var result = _tokenHandler.VerifyJwtSecurityToken(token);
            if (string.IsNullOrEmpty(result)) return false;
            return true;
        }

        public async Task<bool> DeviceExists(Guid number)
        {
            return await _deviceServices.DeviceExists(number);
        }
        public ServerResponse ValidateUser(UserViewModel request)
        {
            var response = ServerResponse.OK;
            if (request.UserTypeId == 0)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(User Type)";
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Name)";
            }
            else if (string.IsNullOrEmpty(request.Email))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Email).";
            }
            else if (string.IsNullOrEmpty(request.MobileNumber))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(MobileNumber).";
            }
            else if (string.IsNullOrEmpty(request.UserName))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(UserName).";
            }
            else if (string.IsNullOrEmpty(request.Password))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Password).";
            }
            return response;
        }

        public async Task<bool> IsPermittedToEditVehicle(string userId, int vehicleId)
        {
            var user = await _vehicleServices.GetUserByVehicle(vehicleId);
            if (user == null) return false;
            if (userId == user.UserId.ToString()) return true;
            return false;
        }

        public async Task<bool> IsPermittedToAddLocation(string userId, int vehicleId)
        {
            var user = await _vehicleServices.GetUserByVehicle(vehicleId);
            if (user == null) return false;
            if (userId == user.UserId.ToString()) return true;
            return false;
        }
        public ServerResponse ValidateVehicle(VehicleViewModel request)
        {
            var response = ServerResponse.OK;
            if (string.IsNullOrEmpty(request.Name))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Name)";
            }
            else if (string.IsNullOrEmpty(request.Model))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Model).";
            }
            else if (string.IsNullOrEmpty(request.Year))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Year).";
            }
            else if (string.IsNullOrEmpty(request.Maker))
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Maker).";
            }
            return response;
        }

        public ServerResponse ValidateLocation(LocationViewModel request)
        {
            var response = ServerResponse.OK;
            if (request.VehicleId == 0)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(VehicleId).";
            }
            else if (request.Latitude == 0)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Latitude).";
            }
            else if (request.Longitude == 0)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Required field is missing(Longitude).";
            }
            return response;
        }
       
    }
}
