using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.api.Business.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }   
        public List<string> Roles { get; set; }

    }
}
