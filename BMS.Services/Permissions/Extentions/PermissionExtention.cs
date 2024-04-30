using BMS.Domain.Models.UserManagement;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Permissions.Extentions
{
    public static class PermissionExtention
    {
        public static bool Has(this List<Permission> permissions, string permissionName)
        {
            return permissions.Any(a => a.Name == permissionName || a.Name == DefaultSitePermissions.admin);
        }

        public static bool HasAny(this List<Permission> permissions, List<string> permissionName)
        {
            return permissions.Any(a => permissionName.Contains(a.Name) || a.Name == DefaultSitePermissions.admin);
        }

        public static bool HasAnyId(this List<Permission> permissions, List<string> permissionIds)
        {
            return permissions.Any(a => permissionIds.Contains(a.Id) || a.Name == DefaultSitePermissions.admin);
        }

        public static bool HasAll(this List<Permission> permissions, List<string> permissionName)
        {
            if (permissions.Any(a => a.Name == DefaultSitePermissions.admin)) return true;
            foreach (var name in permissionName)
                if (!permissions.Any(a => a.Name == name))
                    return false;
            return true;
        }


        public static bool IsLoggedIn(this List<Permission> permissions)
        {
            return permissions.Any(a => a.Name == DefaultSitePermissions.login) || permissions.Any(a => a.Name == DefaultSitePermissions.admin);
        }
    }
}
