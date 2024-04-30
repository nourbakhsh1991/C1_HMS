using BMS.Domain.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Roles.Extentions
{
    public static class ModbusExtentions
    {
        public static Role IncludePermissions(this Role role, Interfaces.IRoleService roleService)
        {
            return roleService.IncludePermissions(role);
        }

        public static List<Role> IncludePermissions(this List<Role> role, Interfaces.IRoleService roleService)
        {
            return roleService.IncludePermissions(role);
        }
    }
}
