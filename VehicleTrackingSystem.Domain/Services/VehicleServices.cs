using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.DataAccess.Repository;
using VehicleTrackingSystem.Entities;

namespace VehicleTrackingSystem.Domain.Services
{
    public class VehicleServices : IVehicleServices
    {
        private readonly ILogger<VehicleServices> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Vehicle> _repository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IMapper _mapper;

        public VehicleServices(ILogger<VehicleServices> logger, IUnitOfWork unitOfWork, IRepository<Vehicle> repository, IRepository<Device> deviceRepository, IMapper mapper )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        public async Task<ServerResponse> AddVehicle(VehicleViewModel entity)
        {
            var response = ServerResponse.OK;
            var model = _mapper.Map<Vehicle>(entity);
            if (entity.VehicleId == 0) await _repository.Add(model);
            else response = await Update(entity, response, model);
            if (response.RespCode == 200) await _unitOfWork.CommitAsync();
            return response;

        }

        public async Task<bool> IsVehicleExists(int vehicleId)
        {
            var resp = await _repository.Get();
            return resp.Any(e => e.VehicleId == vehicleId);
        }

        public async Task<VehicleViewModel> GetUserByVehicle(int vehicleId)
        {
            var resp = await _repository.Get();
            var result = resp.Where(t => t.VehicleId == vehicleId).FirstOrDefault();
            var vechile = _mapper.Map<VehicleViewModel>(result);
            return vechile;
        }
        private async Task<ServerResponse> Update(VehicleViewModel entity, ServerResponse response, Vehicle model)
        {
            var resp = await _repository.Get();
            var vehicle = resp.SingleOrDefault(e => e.VehicleId == entity.VehicleId);
            if (vehicle == null)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "Vehicle is not found";
                return response;
            }
            _repository.Update(model);
            return response;
        }

        public async Task<List<VehicleViewModel>> GetAllVehicles()
        {
            var result = await _repository.Get();
            var list = _mapper.Map<List<VehicleViewModel>>(result);
            return list;
        }

        public async Task<List<VehicleViewModel>> GetAllVehiclesWithDevices()
        {
            var result = (from m in await _repository.Get()
                          join d in await _deviceRepository.Get() on m.DeviceId equals d.DeviceId
                          select new Vehicle
                          {
                              VehicleId = m.VehicleId,
                              Name = m.Name,
                              Maker = m.Maker,
                              Model = m.Model,
                              UserId = m.UserId,
                              Year = m.Year,
                              BodyType = m.BodyType,
                              DeviceId = m.DeviceId,
                              Device = new Device
                              {
                                  DeviceId = d.DeviceId,
                                  ImeiNumber = d.ImeiNumber,
                                  Name = d.Name,
                                  Status = d.Status
                              }
                          }).ToList();
            var list = _mapper.Map<List<VehicleViewModel>>(result);
            return list;
        }
    }
}
