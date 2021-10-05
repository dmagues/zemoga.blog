using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.api.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Post> Posts { get; set; }

    }
}
