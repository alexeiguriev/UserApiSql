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
        public static string[] GetRoleIdsFromUserRoleList(List<UserRole> userRoles)
        {
            string[] roles;
            if (userRoles != null)
            {
                List<string> userRolesNames = new List<string>();

                foreach (UserRole ur in userRoles)
                {
                    userRolesNames.Add(ur.Role.Name);
                }
                roles = userRolesNames.ToArray();
            }
            else
            {
                roles = null;
            }
            return roles;
        }
        public static int[] GetUserIdsFromUserRoleList(List<UserRole> userRoles)
        {
            int[] userIds;
            if (userRoles != null)
            {
                List<int> userRolesIds = new List<int>();

                foreach (UserRole ur in userRoles)
                {
                    userRolesIds.Add(ur.UserId);
                }
                userIds = userRolesIds.ToArray();
            }
            else
            {
                userIds = null;
            }
            return userIds;
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
