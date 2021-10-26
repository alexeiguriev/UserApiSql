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
            List<User> users = new List<User>()
            {
                new User { FirstName = "Guriev", LastName = "Alexei", EmailAddress = "alexeiguriev1@gmail.com", Password = "alexeiTestPassword" },
                new User { FirstName = "Grecu", LastName = "Marin", EmailAddress = "gm@gmail.com", Password = "gmPassword" },
                new User { FirstName = "Milan", LastName = "Iurie", EmailAddress = "mi@Gmail.com", Password = "miPassword" },
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
            List<Document> documents = new List<Document>()
            {
                new Document { Name = "TestDocument1", Type = "PDF", Status = 2, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1)},
                new Document { Name = "TestDocument2", Type = "PDF", Status = 5, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 1) },
                new Document { Name = "TestDocument3", Type = "PDF", Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 2) },
                new Document { Name = "TestDocument4", Type = "PDF", Status = 1, UploadedDate = DateTime.Now, UpdatedBy = context.Users.FirstOrDefault(i => i.Id == 3) }
            };
            foreach (Document document in documents)
            {
                context.Documents.Add(document);
                context.SaveChanges();
            }
        }
        static void RoleInit(UserContext context)
        {
            List<Role> roles = new List<Role>()
            {
                new Role{Name="User"},
                new Role{Name="Admin"},
                new Role{Name="PM"},
                new Role{Name="Developer"},
                new Role{Name="Tester"},
                new Role{Name="Reviewer"}
            };
            foreach (Role role in roles)
            {
                context.Roles.AddRange(role);
                context.SaveChanges();
            }
        }
        static void UserRolesInit(UserContext context)
        {

            List<UserRole> userRoles = new List<UserRole>()
            {
                new UserRole{UserId=1,RoleId=2},
                new UserRole{UserId=2,RoleId=3},
                new UserRole{UserId=3,RoleId=4},
                new UserRole{UserId=4,RoleId=5},
                new UserRole{UserId=5,RoleId=6}
            };
            foreach (UserRole userRole in userRoles)
            {

                context.UserRoles.Add(userRole);
                context.SaveChanges();
            }
            
        }
        public static string Initialize(UserContext context)
        {
            // Look for any users.
            if ((context.Users.Any()) || (context.Documents.Any()) || (context.Roles.Any()) || (context.UserRoles.Any()))
            {
                return "Data already exist";
            }

            UserInitialization(context);
            //DocumentInit(context);
            RoleInit(context);
            UserRolesInit(context);

            return "Data added - done";
        }
    }
}
