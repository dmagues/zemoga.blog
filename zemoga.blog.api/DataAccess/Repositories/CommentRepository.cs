using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
    }
    public class CommentRepository : RepositoryBase<Comment>
    {
        public CommentRepository(BlogContext context) : base(context) { }
    }
}
