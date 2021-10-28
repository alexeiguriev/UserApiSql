using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserApiSql.Models;

namespace UserApiSql.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool sql)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<UserContext>(), sql);
            }
        }

        private static void SeedData(UserContext context, bool sql)
        {
            if(sql)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            else
            {
                UserInitialization(context);
                DocumentInit(context);
                RoleInit(context);
                UserRolesInit(context);
            }
            
        }
        
        static void UserInitialization(UserContext context)
        {
                Console.WriteLine("--> Seeding User Data...");
                context.Users.AddRange(
                    new User { FirstName = "Guriev", LastName = "Alexei", EmailAddress = "alexeiguriev1@gmail.com", Password = "alexeiTestPassword" },
                    new User { FirstName = "Grecu", LastName = "Marin", EmailAddress = "gm@gmail.com", Password = "gmPassword" },
                    new User { FirstName = "Milan", LastName = "Iurie", EmailAddress = "mi@Gmail.com", Password = "miPassword" },
                    new User { FirstName = "Carson", LastName = "Alexander", EmailAddress = "testemail@Gmail.com", Password = "TestPassword" },
                    new User { FirstName = "Popescu", LastName = "Ion", EmailAddress = "ionpopescumail@Gmail.com", Password = "ipPassword" }
                );

                context.SaveChanges();
        }
        static void DocumentInit(UserContext context)
        {
            Console.WriteLine("--> Seeding Document Data...");

            context.Documents.AddRange(
                new Document { Name = "TestDocument1", Type = "PDF", Status = 2, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1)},
                new Document { Name = "TestDocument2", Type = "PDF", Status = 5, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1) },
                new Document { Name = "TestDocument3", Type = "PDF", Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 2) },
                new Document { Name = "TestDocument4", Type = "PDF", Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 3) }
            );

            context.SaveChanges();
        }
        static void RoleInit(UserContext context)
        {
            Console.WriteLine("--> Seeding Role Data...");

            context.Roles.AddRange(
                new Role{Name="User"},
                new Role{Name="Admin"},
                new Role{Name="PM"},
                new Role{Name="Developer"},
                new Role{Name="Tester"},
                new Role{Name="Reviewer"}
            );

                context.SaveChanges();
        }
        static void UserRolesInit(UserContext context)
        {
            Console.WriteLine("--> Seeding Role Data...");
            context.Roles.AddRange(
                new Role{Name="User"},
                new Role{Name="Admin"},
                new Role{Name="PM"},
                new Role{Name="Developer"},
                new Role{Name="Tester"},
                new Role{Name="Reviewer"}
            );

            context.SaveChanges();            
        }
    }
}