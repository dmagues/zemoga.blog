using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        public IRepositoryBase<User> User { get; }
        public IRepositoryBase<Post> Post { get; }
        public IRepositoryBase<UserRole> UserRole { get; }
        public IRepositoryBase<Role> Role { get; }
        public IRepositoryBase<Comment> Comment { get; }
        public void Save();
        public Task SaveAsync();
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly BlogContext _context;
        private IRepositoryBase<User> _user;
        private IRepositoryBase<Post> _post;
        private IRepositoryBase<UserRole> _userRole;
        private IRepositoryBase<Role> _role;
        private IRepositoryBase<Comment> _comment;

        public RepositoryWrapper(BlogContext context)
        {
            _context = context;
        }

        public IRepositoryBase<User> User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }
       
        public IRepositoryBase<Post> Post { 
            get { 
                if(_post == null)
                {
                    _post = new PostRepository(_context);
                }
                return _post;
            }  
        }

        public IRepositoryBase<UserRole> UserRole
        {
            get
            {
                if (_userRole == null)
                {
                    _userRole = new UserRoleRepository(_context);
                }
                return _userRole;
            }
        }


        public IRepositoryBase<Role> Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_context);
                }
                return _role;
            }
        }

        public IRepositoryBase<Comment> Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(_context);
                }
                return _comment;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        
    }
}
