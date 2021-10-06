using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zemoga.blog.webui.Business.DTO.DTO
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime? CommentDate { get; set; }
        public bool IsRejected { get; set; }
    }
}
