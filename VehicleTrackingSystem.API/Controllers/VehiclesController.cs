using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleTrackingSystem.CustomObjects;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.CustomObjects.MapperConfig;
using VehicleTrackingSystem.Domain.Services;
using VehicleTrackingSystem.Validation.Enum;
using VehicleTrackingSystem.Validation.Service;

namespace VehicleTrackingSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleServices _vehicleServices;
        private readonly IGeneralService _generalService;
        private readonly IErrorMapper _errorMapper;

        public VehiclesController(IVehicleServices vehicleServices,IGeneralService generalService, IErrorMapper errorMapper)
        {
            _vehicleServices = vehicleServices;
            _generalService = generalService;
            _errorMapper = errorMapper;
        }

        [AllowAnonymous]
        [HttpPost(Endpoints.RegisterVehicle)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<ActionResult<ServerResponse>> AddVehicle([FromBody] VehicleViewModel model)
        {
            var response = ServerResponse.OK;
            if (string.IsNullOrEmpty(model.Token)) 
                return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.EmptyToken)));
            model.UserId = Convert.ToInt32(_generalService.GetUserFromToken(model.Token));
            response = _generalService.ValidateVehicle(model);
            if (response.RespCode != 200) 
                return Ok(response);
            if (model.VehicleId == 0)
            {
                bool exist = await _generalService.DeviceExists(model.Device.ImeiNumber);
                if (exist) 
                    return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.DeviceExist)));
            }
            else
            {
                if (!await _generalService.IsPermittedToEditVehicle(model.UserId.ToString(), model.VehicleId)) 
                    return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.UnAuthorized)));
            }
            response = await _vehicleServices.AddVehicle(model);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet(Endpoints.GetAllVehicles)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<List<VehicleViewModel>> GetAllVehicles()
        {
            var result = await _vehicleServices.GetAllVehicles();
            return result;
        }
        [AllowAnonymous]
        [HttpGet(Endpoints.GetAllVehiclesWithDevices)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<List<VehicleViewModel>> GetAllVehiclesWithDevices()
        {
            var result = await _vehicleServices.GetAllVehiclesWithDevices();
            return result;
        }
    }
}
