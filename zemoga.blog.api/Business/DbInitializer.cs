using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Models;
using zemoga.blog.api.DataAccess.Infrastructure;

namespace zemoga.blog.api.Business
{
    public class DbInitializer
    {
        public static void Init(BlogContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var roles = new Role[]
            {
                new Role()
                {
                    Name = "Public"
                },
                new Role()
                {
                    Name = "Writer"
                },
                new Role()
                {
                    Name = "Editor"
                }
            };

            var adminUser = new User()
            {
                UserName = "admin",
                Password = "Asdfgh654321"               
                
            };
            var publicUser = new User()
            {
                UserName = "user1",
                Password = "Asdfgh654321"

            };

            foreach (var r in roles)
            {
                context.Roles.Add(r);
            }
            context.Users.Add(adminUser);
            context.Users.Add(publicUser);
            context.SaveChanges();

            var userRoles = new UserRole[]
            {
                new UserRole()
                {
                    UserId = adminUser.UserId,
                    RoleId = roles.Where(r=>r.Name == "Public").Select(r=>r.RoleId).First()
                },
                new UserRole()
                {
                    UserId = adminUser.UserId,
                    RoleId = roles.Where(r=>r.Name == "Writer").Select(r=>r.RoleId).First()
                },
                new UserRole()
                {
                    UserId = adminUser.UserId,
                    RoleId = roles.Where(r=>r.Name == "Editor").Select(r=>r.RoleId).First()
                },
                new UserRole()
                {
                    UserId = publicUser.UserId,
                    RoleId = roles.Where(r=>r.Name == "Public").Select(r=>r.RoleId).First()
                }
            };



            
            foreach (var ur in userRoles)
            {
                context.UsersRoles.Add(ur);
            }

            context.SaveChanges();

        }
    }
}
