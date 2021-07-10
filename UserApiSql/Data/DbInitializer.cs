using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;

namespace UserApiSql.Data
{

    public static class DbInitializer
    {
        static void UserInitialization(UserContext context)
        {
            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            List<User> users = new List<User>()
            {
                new User { FirstName = "Guriev", LastName = "Alexei", EmailAddress = "alexeiemail@Gmail.com", Password = "alexeiTestPassword" },
                new User { FirstName = "Carson", LastName = "Alexander", EmailAddress = "testemail@Gmail.com", Password = "TestPassword" },
                new User { FirstName = "Popescu", LastName = "Ion", EmailAddress = "ionpopescumail@Gmail.com", Password = "ipPassword" }
            };

            foreach (var user in users)
            {

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        static void DocumentInit(UserContext context)
        {
            // Look for any document.
            if (context.Documents.Any())
            {
                return;   // DB has been seeded
            }
            List<Document> documents = new List<Document>()
            {
                new Document { Name = "TestDocument1", Type = 1, Status = 2, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1)},
                new Document { Name = "TestDocument2", Type = 3, Status = 5, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1) },
                new Document { Name = "TestDocument3", Type = 3, Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 2) },
                new Document { Name = "TestDocument4", Type = 3, Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 3) }
            };
            foreach (Document document in documents)
            {
                context.Documents.Add(document);
                context.SaveChanges();
            }
        }
        static void RoleInit(UserContext context)
        {
            // Look for any role.
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }
            List<Role> roles = new List<Role>()
            {
                new Role{Name="Developer"},
                new Role{Name="Tester"},
                new Role{Name="Reviewer"},
                new Role{Name="PM"}
            };
            foreach (Role role in roles)
            {
                context.Roles.AddRange(role);
                context.SaveChanges();
            }
        }
        static void UserRolesInit(UserContext context)
        {
            // Look for any user roles.
            if (context.UserRoles.Any())
            {
                return;   // DB has been seeded
            }

            List<UserRole> userRoles = new List<UserRole>()
            {
                new UserRole{UserId=1,RoleId=1},
                new UserRole{UserId=1,RoleId=3},
                new UserRole{UserId=2,RoleId=2},
                new UserRole{UserId=3,RoleId=4}
            };
            foreach (UserRole userRole in userRoles)
            {

                context.UserRoles.Add(userRole);
                context.SaveChanges();
            }
            
        }
        public static void Initialize(UserContext context)
        {
            UserInitialization(context);
            DocumentInit(context);
            RoleInit(context);
            UserRolesInit(context);
        }
    }
}
