using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zemoga.blog.api.Business;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.DataAccess.Repositories;
using zemoga.blog.api.Services;

namespace zemoga.blog.test
{
    public class ServiceTest
    {
        private BlogContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _context = new BlogContext(options);

            DbInitializer.Init(_context);

        }

        [Test]
        public async Task ItShouldNotDeletePostWhenNotFound()
        {
            var postService = new PostService(new RepositoryWrapper(_context));

            var result = await postService.Delete(1);

            Assert.IsNull(result);

        }
    }
}
