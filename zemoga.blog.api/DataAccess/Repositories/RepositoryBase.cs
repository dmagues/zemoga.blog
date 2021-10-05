using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Helpers;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.DataAccess.Repositories
{

    public interface IRepositoryBase<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        Task<List<T>> GetByConditionWithIncludes(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BlogContext BlogContext { get; set; }
        public RepositoryBase(BlogContext blogContext)
        {
            this.BlogContext = blogContext;
        }
        public async Task<List<T>> GetAll()
        {
            return await this.BlogContext.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await this.BlogContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<List<T>> GetByConditionWithIncludes(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            return await this.BlogContext.Set<T>().Where(expression).IncludeMultiple(includes).ToListAsync();
        }
        public void Create(T entity)
        {
            this.BlogContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this.BlogContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this.BlogContext.Set<T>().Remove(entity);
        }
        

    }
}
