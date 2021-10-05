using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.DataAccess.Repositories;

namespace zemoga.blog.test
{
    public class DataAccessTests
    {
        private BlogContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _context = new BlogContext(options);
            
        }

        [Test]
        public async Task ItShouldCreateOneUserAndGet()
        {
            var repositories = new RepositoryWrapper(_context);

            repositories.User.Create(new User() {                
                UserName = "dmagues"
                ,Password = "123456"                
            });

            await repositories.SaveAsync();

            var users = await repositories.User.GetAll();

            Assert.AreEqual(1, users.Count);
        }

        [Test]
        public async Task ItShouldCreateOnePostAndGet()
        {
            var repositories = new RepositoryWrapper(_context);

            repositories.Post.Create(new Post()
            {
                Content = "post test"
                ,PublishedDate = DateTime.Now
                ,Status = api.PostStatus.Pending
                ,Title = "My new post"                
            });

            await repositories.SaveAsync();

            var posts = await repositories.Post.GetAll();

            Assert.AreEqual(1, posts.Count);
        }
    }
}