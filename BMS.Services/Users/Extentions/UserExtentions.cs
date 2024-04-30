using BMS.Domain.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Users.Extentions
{
    public static class UserExtentions
    {
        public static User IncludeRoles(this User user, Interfaces.IUserService userService)
        {
            return userService.IncludeRoles(user);
        }

        public static List<User> IncludeRoles(this List<User> user, Interfaces.IUserService userService)
        {
            return userService.IncludeRoles(user);
        }

        public static List<Permission> GetPermissions(this User user)
        {
            var permissions = new List<Permission>();
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    if (role.Permissions != null)
                        permissions.AddRange(role.Permissions);
                }
            }
            return permissions;
        }
    }
}
