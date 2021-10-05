using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.api.Business.DTO
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public PostStatus Status { get; set; }
        public string Author { get; set; }
    }
}
