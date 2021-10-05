using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(BlogContext context) : base(context) { }

        
    }
}
