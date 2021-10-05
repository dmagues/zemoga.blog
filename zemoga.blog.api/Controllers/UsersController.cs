using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Business.DTO;
using zemoga.blog.api.Models;
using zemoga.blog.api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zemoga.blog.api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }
        // GET: api/<UsersController>
        [HttpGet]        
        public async Task<UserDTO> Login(string userName, string password)
        {
            return await this._userService.VerifyCredentials(userName, password);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Register([FromBody] UserRegisterDTO value)
        {
            this._userService.Register(value);
        }

       
    }
}
