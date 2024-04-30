using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Helpers
{
    public static class DefaultSitePermissions
    {
        // Common
        public static string admin => "__admin__";
        public static string login => "__login__";

        // Map
        public static string accessMaps => "__access_maps__";
        public static string createMap => "__create_map__";
        public static string editLayers => "__edit_layers__";

        // Draw
        public static string accessDraw => "__access_draw__";
        public static string accessEdit => "__access_edit__";

        // Modbus
        public static string accessModbus => "__access_modbus__";
        public static string createModbus => "__create_modbus__";

        // User Management
        public static string accessUsers => "__access_users__";
        public static string createUser => "__create_user__";
        public static string accessRoles => "__access_roles__";
        public static string createRole => "__create_role__";
        public static string accessPermissions => "__access_permissions__";
        public static string createPermission => "__create_permission__";
    }
}
