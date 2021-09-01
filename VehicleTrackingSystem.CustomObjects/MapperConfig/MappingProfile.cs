using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.Entities;

namespace VehicleTrackingSystem.CustomObjects.MapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserType, UserTypeViewModel>();
            CreateMap<UserTypeViewModel, UserType>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<Vehicle, VehicleViewModel>();
            CreateMap<VehicleViewModel, Vehicle>();
            CreateMap<Device, DeviceViewModel>();
            CreateMap<DeviceViewModel, Device>();
            CreateMap<Location, LocationViewModel>();
            CreateMap<LocationViewModel, Location>();
        }
    }
}
