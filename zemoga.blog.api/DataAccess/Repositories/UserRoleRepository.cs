using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {

    }
    public class UserRoleRepository: RepositoryBase<UserRole>
    {
        public UserRoleRepository(BlogContext context) : base(context) { }
    }
}
