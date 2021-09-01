using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleTrackingSystem.CustomObjects;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.CustomObjects.Request;
using VehicleTrackingSystem.Domain.Services;
using VehicleTrackingSystem.Validation.Enum;
using VehicleTrackingSystem.Validation.Service;

namespace VehicleTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGeneralService _generalService;
        private readonly IUserServices _userServices;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IGeneralService generalService, IUserServices userServices, ILogger<AuthController> logger)
        {
            _generalService = generalService;
            _userServices = userServices;
            _logger = logger;
        }

        [HttpPost(Endpoints.Login)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<ActionResult<UserViewModel>> Login([FromBody] LoginObject model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return Ok(new ServerResponse { RespCode = 500, RespDesc = new EnumDescription().StringValueOfEnum(ResponseEnum.BlankUsernamePassword) });
            var user = await _userServices.Authenticate(model.Username, model.Password);
            if (user == null) return Ok(user);
            return Ok(user);
        }

        [HttpPost(Endpoints.Registration)]
        [Consumes(ServiceConsumesType.Json)]
        public async Task<ActionResult<ServerResponse>> Register([FromBody] UserViewModel model)
        {

            var response = ServerResponse.OK;
            response = _generalService.ValidateUser(model);
            if (response.RespCode != 200) return Ok(response);
            if (model.UserId == 0)
            {
                bool exist = await _generalService.UserExists(model.UserName);
                if (exist) return Ok(new ServerResponse { RespCode = 500, RespDesc = new EnumDescription().StringValueOfEnum(ResponseEnum.UserExist) });
            }
            response = await _userServices.AddUser(model);
            return Ok(response);
        }
    }
}
