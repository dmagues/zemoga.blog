using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IBlogRepository : IRepositoryBase<Blog> { }
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(BlogContext context): base(context) { }
    }
}
