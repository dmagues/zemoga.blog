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
        

        /// <summary>
        /// Register a new User passing username, password and roles
        /// For Roles, you can set an int array of roles with the following values:
        /// 1 - Public 
        /// 2 - Writer 
        /// 3 - Editor
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        public async Task Register(UserRegisterDTO userRegister)
        {
            var user = new User()
            {
                UserName = userRegister.Username,
                Password = userRegister.Password
            };

            this._repository.User.Create(user);
            await this._repository.SaveAsync();
            // add public as default            
            var defaultUserRole = new UserRole() { UserId = user.UserId, RoleId = 1 };
            this._repository.UserRole.Create(defaultUserRole);
            await this._repository.SaveAsync();

            var userRolesAdded = new List<UserRole>
            {
                defaultUserRole
            };

            var roles = await this._repository.Role.GetAll();

            foreach (var r in userRegister.Roles)
            {
                if (!userRolesAdded.Any(ur => ur.RoleId == r) && roles.Any(role=>role.RoleId == r))
                {
                    
                    var userRole = (new UserRole()
                    {
                        UserId = user.UserId,
                        RoleId = r
                    });
                    userRolesAdded.Add(userRole);
                    this._repository.UserRole.Create(userRole);
                    
                }                
                
            }
            await this._repository.SaveAsync();

        }

        /// <summary>
        /// Verify the credentials of a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
