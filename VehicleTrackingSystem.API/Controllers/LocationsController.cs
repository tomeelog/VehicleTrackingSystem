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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationServices _locationServices;
        private readonly IGeneralService _generalService;
        private readonly IErrorMapper _errorMapper;

        public LocationsController(ILocationServices locationServices, IGeneralService generalService, IErrorMapper errorMapper)
        {
            _locationServices = locationServices;
            _generalService = generalService;
            _errorMapper = errorMapper;
        }

        [AllowAnonymous]
        [HttpPost(Endpoints.RecordLocation)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<ActionResult> RecordLocation([FromBody] LocationViewModel model)
        {
            var response = ServerResponse.OK;
            if (string.IsNullOrEmpty(model.Token)) 
                return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.EmptyToken)));

            if (!_generalService.IsValidToken(model.Token)) 
                return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.InvalidToken)));

            response = _generalService.ValidateLocation(model);
            if (response.RespCode != 200) return Ok(response);

            if (!await _generalService.VehicleExists(model.VehicleId)) 
                return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.NotFound)));

            var userId = _generalService.GetUserFromToken(model.Token);
            if (!await _generalService.IsPermittedToAddLocation(userId.ToString(), model.VehicleId)) 
                return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.UnAuthorized)));

            response = await _locationServices.RecordLocation(model);
            return Ok(response);
        }
       
        [Authorize(Roles = AllowAccess.Admin)]
        [HttpPost(Endpoints.GetCurrentLocation)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<ActionResult> GetCurrentLocation([FromBody] LocationViewModel model)
        {
            var response = ServerResponse.OK;
            if (string.IsNullOrEmpty(model.Token)) return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.InvalidToken)));
            if (!await _generalService.IsAdministrator(model.Token)) return Ok(_errorMapper.MapToError(ServerResponse.BadRequest, new EnumDescription().StringValueOfEnum(ResponseEnum.UnAuthorized)));
            response.Result = await _locationServices.RetriveLocation(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Endpoints.GetAllLocations)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<List<LocationViewModel>> GetAllLocations()
        {
            var result = await _locationServices.GetAllLocations();
            return result;
        }
    }
}
