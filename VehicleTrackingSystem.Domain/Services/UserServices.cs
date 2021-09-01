using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTrackingSystem.CustomObjects.Abstraction;
using VehicleTrackingSystem.CustomObjects.Domain;
using VehicleTrackingSystem.CustomObjects.MapperConfig;
using VehicleTrackingSystem.DataAccess.Repository;
using VehicleTrackingSystem.Entities;
using VehicleTrackingSystem.Security.Handlers;

namespace VehicleTrackingSystem.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<UserType> _repository;
        private readonly IRepository<User> _userRepository;
        private readonly ICryptographyHandler _cryptographyHandler;
        private readonly IErrorMapper _errorMapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;

        public UserServices(IUnitOfWork unitOfWork, IMapper mapper, IRepository<UserType> repository,
            IRepository<User> userRepository, ICryptographyHandler cryptographyHandler, IErrorMapper  errorMapper, IJwtTokenHandler jwtTokenHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
            _cryptographyHandler = cryptographyHandler;
            _errorMapper = errorMapper;
            _jwtTokenHandler = jwtTokenHandler;
        }

        public async Task<UserViewModel> Authenticate(string username, string password)
        {
            var response = ServerResponse.OK;
            var resp = await _userRepository.Get();
            var result = resp.SingleOrDefault(x => x.UserName == username);
            var RoleResult = await _repository.Get();
            var Role = RoleResult.SingleOrDefault(x => x.UserTypeId == result.UserTypeId);
            if (result == null) return _errorMapper.MapToError(null, ServerResponse.BadRequest, "User is not found.");
            bool isValid = _cryptographyHandler.VerifyGeneratedHash(password, result.Password);
            if (!isValid) return _errorMapper.MapToError(null, ServerResponse.BadRequest, "Username or password is incorrect.");
            var user = _mapper.Map<UserViewModel>(result);
            user.Token = _jwtTokenHandler.GenerateJwtSecurityToken(user.UserId.ToString(), Role.Name);
            user.Password = null;
            return user;
        }
        public async Task AddUserTypes(UserTypeViewModel entity)
        {
            var model = _mapper.Map<UserType>(entity);
            await _repository.Add(model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ServerResponse> AddUser(UserViewModel entity)
        {
            var response = ServerResponse.OK;
            if (entity.UserId == 0) entity.Password = _cryptographyHandler.GeneratePasswordHash(entity.Password);
            var model = _mapper.Map<User>(entity);
            if (entity.UserId == 0) await _userRepository.Add(model);
            else response = await Update(entity, response, model);
            if (response.RespCode == 200) await _unitOfWork.CommitAsync();
            return response;
        }

        public async Task<long> GetUserTypeByUser(int userId)
        {
            var resp = await _userRepository.Get();
            return resp.FirstOrDefault(u => u.UserId == userId).UserTypeId;
        }
        private async Task<ServerResponse> Update(UserViewModel entity, ServerResponse response, User model)
        {
            var user = await _userRepository.Get();
            var usr = user.SingleOrDefault(e => e.UserId == entity.UserId);
            //SingleOrDefault(e => e.UserId == entity.UserId)
            if (user == null)
            {
                response = ServerResponse.BadRequest;
                response.RespDesc = "User is not found";
                return response;
            }
            model.UserName = usr.UserName;
            model.Password = usr.Password;
            _userRepository.Update(model);
            return response;
        }

        public async Task<long> GetUserTypeByUser(long userId)
        {
            var resp = await _userRepository.Get();
            return resp.FirstOrDefault(u => u.UserId == userId).UserTypeId;
        }

        public async Task<bool> UserExists(string userName)
        {
            var resp = await _userRepository.Get();
            return resp.Any(e => e.UserName == userName);
        }
    }
}
