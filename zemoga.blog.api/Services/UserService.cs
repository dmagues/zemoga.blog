using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Business.DTO;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Repositories;

namespace zemoga.blog.api.Services
{
    public interface IUserService
    {
        Task Register(UserRegisterDTO userRegister);
        
        Task<UserDTO> VerifyCredentials(string username, string password);

    }
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repository;
        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;
        }
        

        public async Task Register(UserRegisterDTO userRegister)
        {
            var user = new User()
            {
                UserName = userRegister.Username,
                Password = userRegister.Password
            };

            this._repository.User.Create(user);
            await this._repository.SaveAsync();         
            
            foreach (var r in userRegister.Roles)
            {
                var userRole = (new UserRole()
                {
                    UserId = user.UserId,
                    RoleId = r
                });
                this._repository.UserRole.Create(userRole);
            }
            await this._repository.SaveAsync();

        }

        public async Task<UserDTO> VerifyCredentials(string username, string password)
        {
            var user =  await _repository.User.GetByCondition(u => u.UserName == username && u.Password == password);
            if (user != null && user.Any())
            {
                var userDto = user.Select(u => new UserDTO()
                {
                    UserId = u.UserId,
                    Username = u.UserName

                }).FirstOrDefault();
                var userRoles = await _repository.UserRole.GetByCondition(ur => ur.UserId == userDto.UserId);
                var roles = await _repository.Role.GetAll();
                userDto.Roles = new List<string>();
                foreach (var ur in userRoles)
                {
                    var role = roles.Where(m => m.RoleId == ur.RoleId).First();
                    userDto.Roles.Add(role.Name);
                }

                return userDto;
            }

            return null;

        }

       
    }
}
