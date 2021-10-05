using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zemoga.blog.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        [Authorize]
        public IEnumerable<User> Get()
        {
            return Array.Empty<User>();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User Get(Guid id)
        {
            return null;
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] User value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] User value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
