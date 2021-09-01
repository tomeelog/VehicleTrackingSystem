using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.DataAccess.Repository;
using VehicleTrackingSystem.Entities;

namespace VehicleTrackingSystem.Domain.Services
{
    public class DeviceServices : IDeviceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Device> _repository;

        public DeviceServices(IUnitOfWork unitOfWork, IRepository<Device> repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

  
        public async Task<bool> DeviceExists(Guid number)
        {
            var resp = await _repository.Get();
            return resp.Any(e => e.ImeiNumber == number);
        }
        public async Task<List<DeviceViewModel>> GetAllDevices()
        {
            var result = await _repository.Get();
            var list = _mapper.Map<List<DeviceViewModel>>(result);
            return list;
        }
        public async Task AddDevice(DeviceViewModel entity)
        {
            var model = _mapper.Map<Device>(entity);
            await _repository.Add(model);
            await _unitOfWork.CommitAsync();
        }
    }
}
