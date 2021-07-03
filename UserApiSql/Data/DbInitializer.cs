using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;

namespace UserApiSql.Data
{

    public static class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{FirstName="Guriev",LastName="Alexei",EmailAddress="alexeiemail@Gmail.com",Password="alexeiTestPassword"},
                new User{FirstName="Carson",LastName="Alexander",EmailAddress="testemail@Gmail.com",Password="TestPassword"},
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
