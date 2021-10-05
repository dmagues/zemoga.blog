using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IRoleRepository:IRepositoryBase<Role>
    {

    }
    public class RoleRepository : RepositoryBase<Role>

    {
        public RoleRepository(BlogContext context) : base(context) { }
    }
}
