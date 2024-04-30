using BMS.Domain.ClientModels;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Users
{
    public class UserService : IUserService
    {
        IRepository<User> _repository;
        IRepository<Role> _repositoryRole;
        IRepository<Permission> _repositoryPermission;

        public UserService(
            IRepository<User> repository,
            IRepository<Role> repositoryRole,
            IRepository<Permission> repositoryPermission
            )
        {
            _repository = repository;
            _repositoryRole = repositoryRole;
            _repositoryPermission = repositoryPermission;
        }

        public async Task Insert(User user)
        {
            if (user == null)
                throw new Exception();
            user.Id = null;
            user.CreatedTime = DateTime.Now;
            user.LastLogin = DateTime.Now;
            await _repository.InsertAsync(user);
        }

        public async Task Insert(List<User> users)
        {
            if (users == null)
                throw new Exception();
            await _repository.InsertAsync(users);
        }

        public async Task Update(User user)
        {
            if (user == null)
                throw new NullReferenceException("user cannot be null");
            if (!user.CanEdit)
                throw new AccessViolationException("Can not be edited");
            await _repository.UpdateAsync(user);
        }

        public async Task UpdateLogin(User user)
        {
            if (user == null)
                throw new NullReferenceException("user cannot be null");
            await _repository.UpdateAsync(user);
        }

        public async Task Delete(User user)
        {
            if (user == null)
                throw new NullReferenceException("user cannot be null");

            if (!user.CanDelete)
                throw new AccessViolationException("Can not be deleted");
            await _repository.DeleteAsync(user);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("Id cannot be null");
            var user = GetById(Id);
            if (user == null)
                throw new NullReferenceException("user cannot be null");
            if (!user.CanDelete)
                throw new AccessViolationException("Can not be deleted");

            await _repository.DeleteAsync(user);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public User GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Permission> GetPermissionsByClaims(System.Security.Claims.ClaimsPrincipal user)
        {
            var permissions = new List<Permission>();
            var usernameClaim = user.Claims.FirstOrDefault(a => a.Type == "username");
            if (usernameClaim == null) return permissions;
            var username = usernameClaim.Value;
            var _user = GetByUsername(username);
            if (_user == null) return permissions;
            if (_user.RoleIds == null || !_user.RoleIds.Any()) return permissions;
            _user = IncludeRoles(_user);
            foreach (var role in _user.Roles)
            {
                if (role.Permissions != null)
                    permissions.AddRange(role.Permissions);
            }
            permissions = permissions.DistinctBy(a=>a.Id).ToList();
            return permissions;
        }

        public User GetByUsername(string name)
        {
            return _repository.Table.Where(x => x.Username.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public User IncludeRoles(User user)
        {
            if (user == null) throw new NullReferenceException("role cannot be null");
            var users = IncludeRoles(new List<User> { user });
            if (users != null && users.Count > 0)
                return users.First();
            return user;
        }

        public List<User> IncludeRoles(List<User> users)
        {
            if (users == null) throw new NullReferenceException("role cannot be null");
            if (users.Count == 0) return users;
            var roleIds = users.Aggregate(new List<string>(), (list, current) =>
            {
                if (current != null && current.RoleIds != null)
                    list.AddRange(current.RoleIds);
                return list;
            });
            var roles = _repositoryRole.Table.Where(a => roleIds.Contains(a.Id)).ToList();
            foreach (var user in users)
            {
                user.Roles = new List<Role>();
                if (user.RoleIds != null)
                {
                    foreach (var id in user.RoleIds)
                    {
                        var role = roles.FirstOrDefault(a => a.Id == id);
                        if (role == null) continue;
                        IncludePermissions(role);
                        user.Roles.Add(role);
                    }
                }
            }
            return users;
        }

        private Role IncludePermissions(Role role)
        {
            if (role == null) throw new NullReferenceException("role cannot be null");
            var roles = IncludePermissions(new List<Role> { role });
            if (roles != null && roles.Count > 0)
                return roles.First();
            return role;
        }

        private List<Role> IncludePermissions(List<Role> roles)
        {
            if (roles == null) throw new NullReferenceException("role cannot be null");
            if (roles.Count == 0) return roles;
            var permissonIds = roles.Aggregate(new List<string>(), (list, current) =>
            {
                if (current != null && current.PermissionIds != null)
                    list.AddRange(current.PermissionIds);
                return list;
            });
            var permissons = _repositoryPermission.Table.Where(a => permissonIds.Contains(a.Id)).ToList();
            foreach (var role in roles)
            {
                role.Permissions = new List<Permission>();
                if (role.PermissionIds != null)
                {
                    foreach (var id in role.PermissionIds)
                    {
                        var permission = permissons.FirstOrDefault(a => a.Id == id);
                        if (permission == null) continue;
                        role.Permissions.Add(permission);
                    }
                }
            }
            return roles;
        }

        public List<User> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<User> GetAsQueryable()
        {
            return _repository.Table;
        }
    }
}
