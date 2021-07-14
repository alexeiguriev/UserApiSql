using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;

namespace UserApiSql.Helper
{
    static class Helper
    {
        public static byte[] PdfToByteBuffConverter(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
        public static string[] ListOfUsersToUserString(List<UserRole> userRoles)
        {
            if (userRoles == null)
            {
                return null;
            }
            List<string> usersList = new List<string>();
            foreach(UserRole userRole in userRoles)
            {
                usersList.Add($"{ userRole.User.FirstName } { userRole.User.LastName}");
                
            }
            string[] outString = usersList.ToArray();
            return outString;
        }

    }
}
