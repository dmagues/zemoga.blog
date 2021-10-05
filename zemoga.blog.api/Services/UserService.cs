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
        public User Register(UserRegisterDTO userRegister);
        public User Login(UserLoginDTO userLogin);
        public Task<User> Validate(string username, string password);
        public Task<List<UserRole>> UserInRole(int userId);


    }
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repository;
        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;
        }
        public User Login(UserLoginDTO userLogin)
        {
            throw new NotImplementedException();
        }

        public User Register(UserRegisterDTO userRegister)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Validate(string username, string password)
        {
            var user =  await _repository.User.GetByCondition(u => u.UserName == username && u.Password == password);
            if(user != null && user.Any())
            {
                 
                return user.First();
            }

            return null;

        }

        public async Task<List<UserRole>> UserInRole(int userId)
        {

            var userRoles = await _repository.UserRole.GetByCondition(ur => ur.UserId == userId);
            var roles = await _repository.Role.GetAll();
            
            foreach(var ur in userRoles)
            {
                var role = roles.Where(m => m.RoleId == ur.RoleId).First();
                ur.Role = role;
            }
            return userRoles;

        }
    }
}
