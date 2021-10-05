using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
    }
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(BlogContext context) : base(context) { }
    }
}
