using AutoMapper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.CustomObjects.Settings;
using VehicleTrackingSystem.DataAccess.Repository;
using VehicleTrackingSystem.Entities;
using VehicleTrackingSystem.Utilities.ExternalService;

namespace VehicleTrackingSystem.Domain.Services
{
    public class LocationServices : ILocationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _repository;
        private readonly IMapper _mapper;
        private readonly IExternalService _externalService;
        private readonly AppSettings _appSettings;

        public LocationServices(IUnitOfWork unitOfWork, IRepository<Location> repository, IMapper mapper, IExternalService externalService, AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
           _externalService = externalService;
           _appSettings = appSettings;
        }

        public async Task<List<LocationViewModel>> GetAllLocations()
        {
            var result = await _repository.Get();
            var list = _mapper.Map<List<LocationViewModel>>(result);
            return list;
        }

        public async Task<ServerResponse> RecordLocation(LocationViewModel entity)
        {
            entity.LocationId = 0;
            var response = ServerResponse.OK;
            var model = _mapper.Map<Location>(entity);
            await _repository.Add(model);
            await _unitOfWork.CommitAsync();
            return response;
        }

        public async Task<List<LocationViewModel>> RetriveLocation(LocationViewModel entity)
        {
            var result = (IEnumerable<Location>)null;
            if (entity.VehicleId == 0) result = await _repository.Get();
            else if (entity.DateFrom != DateTime.MinValue && entity.DateTo != DateTime.MinValue) result = await LocationBetweenDates(entity);
            else result = await CurrentLocation(entity);
            var response = _mapper.Map<List<LocationViewModel>>(result);
            var responseWithLocality = await GetLocalityFromService(response);
            return responseWithLocality;
        }


        private async Task<IEnumerable<Location>> CurrentLocation(LocationViewModel entity)
        {
            var resp = await _repository.Get();
            return resp.Where(t => t.VehicleId == entity.VehicleId).OrderByDescending(x => x.CreatedTime).Take(1);
        }

        private async Task<IEnumerable<Location>> LocationBetweenDates(LocationViewModel entity)
        {
            return (from a in await _repository.Get()
                    where a.VehicleId == entity.VehicleId &&
                    (a.CreatedTime >= entity.DateFrom && a.CreatedTime <= entity.DateTo)
                    select a).ToList();
        }

        private async Task<List<LocationViewModel>> GetLocalityFromService(List<LocationViewModel> list)
        {

            foreach (var locality in list)
            {
                string baseUrl = _appSettings.LocationUrl;
                string url = baseUrl+$"/REST/v1/Locations/{locality.Latitude},{locality.Longitude}?key={_appSettings.ApiKey}";
                var resp = await _externalService.CallServiceAsync<LocationObject>(Method.GET, url, null, true) as LocationObject;
                locality.Locality = resp.resourceSets.FirstOrDefault().resources.FirstOrDefault().address.locality;
            }
            return list;
        }
    }
}
