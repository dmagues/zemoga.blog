using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{

    public interface IRepositoryBase<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BlogContext _blogContext { get; set; }
        public RepositoryBase(BlogContext blogContext)
        {
            this._blogContext = blogContext;
        }
        public async Task<List<T>> GetAll()
        {
            return await this._blogContext.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await this._blogContext.Set<T>().Where(expression).ToListAsync();
        }
        public void Create(T entity)
        {
            this._blogContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this._blogContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this._blogContext.Set<T>().Remove(entity);
        }
    }
}
