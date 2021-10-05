using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.webui.Business
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int[] Roles { get; set; }
    }
}
