using Microsoft.Extensions.DependencyInjection;
using BMS.Domain.Interfaces;
using BMS.Domain.Menues;
using BMS.Domain.Models;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Menus.Interfaces;
using BMS.Services.Permissions.Interfaces;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Services
{
    public class InititalDataGenerator
    {
        public void Generate(IServiceProvider provider)
        {
            var startuplog = provider.GetRequiredService<IRepository<StartupLog>>();
            var menuService = provider.GetRequiredService<IMenuService>();
            var permissionService = provider.GetRequiredService<IPermissionService>();
            var roleService = provider.GetRequiredService<IRoleService>();
            var userService = provider.GetRequiredService<IUserService>();

            StartupLog log = new StartupLog();
            log.dateTime = DateTime.UtcNow;
            log.Metadata.Add("Detail", "First time install database");
            log.Metadata.Add("Here", "Here");

            startuplog.Insert(log);

            // Menus
            var dashboard = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/dashboard.svg",
                Name = "داشبورد",
                RouterLink = "/dashboard",
                RouterLinkActive = "active",
            };

            var userManagement = new AsideMenuItem
            {
                HasSubMenu = true,
                IsSeparator = false,
                Icon = "./assets/media/icons/usermanagement.svg",
                Name = "مدیریت کاربران",
                RouterLinkActive = "here show",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }

            };
            var users = new AsideMenuItem
            {
                HasSubMenu = true,
                IsSeparator = false,
                Icon = "./assets/media/icons/user.svg",
                Name = "کاربران",
                RouterLinkActive = "here show",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }

            };
            var usersList = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/list.svg",
                Name = "فهرست کاربران",
                RouterLinkActive = "active",
                RouterLink = "/users/list",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };
            var createUser = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/new.svg",
                Name = "ایجاد کاربر جدید",
                RouterLinkActive = "active",
                RouterLink = "/users/create",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };
            var roles = new AsideMenuItem
            {
                HasSubMenu = true,
                IsSeparator = false,
                Icon = "./assets/media/icons/role.svg",
                Name = "نقش ها",
                RouterLinkActive = "here show",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }

            };
            var rolesList = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/list.svg",
                Name = "فهرست نقش ها",
                RouterLinkActive = "active",
                RouterLink = "/roles/list",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };
            var createRole = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/new.svg",
                Name = "ایجاد نقش جدید",
                RouterLinkActive = "active",
                RouterLink = "/users/create",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };

            var permissions = new AsideMenuItem
            {
                HasSubMenu = true,
                IsSeparator = false,
                Icon = "./assets/media/icons/permission.svg",
                Name = "دسترسی ها",
                RouterLinkActive = "here show",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }

            };
            var permissionsList = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/list.svg",
                Name = "فهرست دسترسی ها",
                RouterLinkActive = "active",
                RouterLink = "/permissions/list",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };
            var createPermission = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/new.svg",
                Name = "ایجاد دسترسی جدید",
                RouterLinkActive = "active",
                RouterLink = "/permissions/create",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };

            var backupAndRestore = new AsideMenuItem
            {
                HasSubMenu = true,
                IsSeparator = false,
                Icon = "./assets/media/icons/sync.svg",
                Name = "پشتیبان",
                RouterLinkActive = "here show",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }

            };
            var backup = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/backup.svg",
                Name = "پشتیبان گیری",
                RouterLinkActive = "active",
                RouterLink = "/backup/backup",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };
            var restore = new AsideMenuItem
            {
                HasSubMenu = false,
                IsSeparator = false,
                Icon = "./assets/media/icons/restore.svg",
                Name = "بازیابی پشتیبان",
                RouterLinkActive = "active",
                RouterLink = "/backup/restore",
                PermissionNames = new List<string> { DefaultSitePermissions.admin }
            };

            userManagement.SubMenuIds.Add(users.Id);
            userManagement.SubMenuIds.Add(roles.Id);
            userManagement.SubMenuIds.Add(permissions.Id);
            users.SubMenuIds.Add(usersList.Id);
            users.SubMenuIds.Add(createUser.Id);
            roles.SubMenuIds.Add(rolesList.Id);
            roles.SubMenuIds.Add(createRole.Id);
            permissions.SubMenuIds.Add(permissionsList.Id);
            permissions.SubMenuIds.Add(createPermission.Id);

            backupAndRestore.SubMenuIds.Add(backup.Id);
            backupAndRestore.SubMenuIds.Add(restore.Id);

            menuService.Insert(dashboard);

            menuService.Insert(userManagement);
            menuService.Insert(users);
            menuService.Insert(usersList);
            menuService.Insert(createUser);
            menuService.Insert(roles);
            menuService.Insert(rolesList);
            menuService.Insert(createRole);
            menuService.Insert(permissions);
            menuService.Insert(permissionsList);
            menuService.Insert(createPermission);

            menuService.Insert(backupAndRestore);
            menuService.Insert(backup);
            menuService.Insert(restore);

            // Default Permissions
            var adminPermission = new Permission
            {
                Name = DefaultSitePermissions.admin,
                CanEdit = false,
                CanDelete = false,
                Description = "Administrator Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var loginPermission = new Permission
            {
                Name = DefaultSitePermissions.login,
                CanEdit = false,
                CanDelete = false,
                Description = "Login Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessMapsPermission = new Permission
            {
                Name = DefaultSitePermissions.accessMaps,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Maps Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var createMapPermission = new Permission
            {
                Name = DefaultSitePermissions.createMap,
                CanEdit = false,
                CanDelete = false,
                Description = "Create Maps Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var editMapLayersPermission = new Permission
            {
                Name = DefaultSitePermissions.editLayers,
                CanEdit = false,
                CanDelete = false,
                Description = "Edit Map Layers Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessDrawPermission = new Permission
            {
                Name = DefaultSitePermissions.accessDraw,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Map Draw Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessEditPermission = new Permission
            {
                Name = DefaultSitePermissions.accessEdit,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Map Edit Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessModbusPermission = new Permission
            {
                Name = DefaultSitePermissions.accessModbus,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Modbus Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var createModbusPermission = new Permission
            {
                Name = DefaultSitePermissions.createModbus,
                CanEdit = false,
                CanDelete = false,
                Description = "Create Modbus Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessUsersPermission = new Permission
            {
                Name = DefaultSitePermissions.accessUsers,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Users Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var createUserPermission = new Permission
            {
                Name = DefaultSitePermissions.createUser,
                CanEdit = false,
                CanDelete = false,
                Description = "Create User Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessRolesPermission = new Permission
            {
                Name = DefaultSitePermissions.accessRoles,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Roles Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var createRolePermission = new Permission
            {
                Name = DefaultSitePermissions.createRole,
                CanEdit = false,
                CanDelete = false,
                Description = "Create Role Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var accessPermissionsPermission = new Permission
            {
                Name = DefaultSitePermissions.accessPermissions,
                CanEdit = false,
                CanDelete = false,
                Description = "Access Permissions Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };
            var createPermissionPermission = new Permission
            {
                Name = DefaultSitePermissions.createPermission,
                CanEdit = false,
                CanDelete = false,
                Description = "Create Permission Permission",
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };

            permissionService.Insert(adminPermission);
            permissionService.Insert(accessMapsPermission);
            permissionService.Insert(createMapPermission);
            permissionService.Insert(editMapLayersPermission);
            permissionService.Insert(accessDrawPermission);
            permissionService.Insert(accessEditPermission);
            permissionService.Insert(accessModbusPermission);
            permissionService.Insert(createModbusPermission);
            permissionService.Insert(accessUsersPermission);
            permissionService.Insert(createUserPermission);
            permissionService.Insert(accessRolesPermission);
            permissionService.Insert(createRolePermission);
            permissionService.Insert(accessPermissionsPermission);
            permissionService.Insert(createPermissionPermission);

            // Default Role
            var adminRole = new Role
            {
                Name = "__admin__",
                Description = "System Administrator",
                CanDelete = false,
                CanEdit = false,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };

            adminRole.PermissionIds.Add(adminPermission.Id);
            adminRole.PermissionIds.Add(loginPermission.Id);

            roleService.Insert(adminRole);

            // Default User
            var admin = new User
            {
                Username = "admin",
                FirstName = "",
                LastName = "",
                Password = "56ab62e763378504fc4df40f0b25b5c3".ToUpper(),
                CanDelete = false,
                CanEdit = false,
                CreatedTime = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
            };

            admin.RoleIds.Add(adminRole.Id);

            userService.Insert(admin);

        }
    }
}
