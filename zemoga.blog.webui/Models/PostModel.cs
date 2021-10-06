using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.webui.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        [Required]
        [MaxLength(50)]        
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Content { get; set; }
        [Required]
        public DateTime? PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public int Status { get; set; }
        public string Author { get; set; }
        public List<object> Comments { get; set; }
    }
}
