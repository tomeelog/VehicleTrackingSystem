using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;

namespace VehicleTrackingSystem.Domain.Services
{
    public interface IUserServices
    {
        Task<UserViewModel> Authenticate(string username, string password);
        Task<ServerResponse> AddUser(UserViewModel entity);
        Task<bool> UserExists(string userName);
        Task AddUserTypes(UserTypeViewModel entity);
        Task<long> GetUserTypeByUser(int userId);
    }
}
