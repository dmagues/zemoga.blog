using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.webui.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]        
        public string Password { get; set; }        
        public List<string> Roles { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberLogin { get; set; }
    }
}
