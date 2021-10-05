using Microsoft.EntityFrameworkCore;
using System;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.DataAccess.Infrastructure
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogContext()
        {            
        }

        public BlogContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(b => b.Comments)
                .WithOne(e => e.Post);
                            
        }
    }
}

