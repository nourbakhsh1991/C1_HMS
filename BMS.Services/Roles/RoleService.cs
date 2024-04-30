using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Roles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Roles
{
    public class RoleService : IRoleService
    {
        IRepository<Role> _repository;
        IRepository<Permission> _repositoryPermission;

        public RoleService(IRepository<Role> repository,
            IRepository<Permission> repositoryPermission)
        {
            _repository = repository;
            _repositoryPermission = repositoryPermission;
        }

        public async Task Insert(Role Role)
        {
            if (Role == null)
                throw new Exception();
            Role.Id = null;
            Role.CreatedTime = DateTime.Now;
            Role.UpdatedTime = DateTime.Now;
            await _repository.InsertAsync(Role);
        }

        public async Task Insert(List<Role> Roles)
        {
            if (Roles == null)
                throw new Exception();
            await _repository.InsertAsync(Roles);
        }

        public async Task Update(Role Role)
        {
            if (Role == null)
                throw new NullReferenceException("Role cannot be null");
            if (!Role.CanEdit)
                throw new AccessViolationException("Can not be edited");
            Role.UpdatedTime = DateTime.Now;
            await _repository.UpdateAsync(Role);
        }

        public async Task Delete(Role Role)
        {
            if (Role == null)
                throw new NullReferenceException("Role cannot be null");

            if (!Role.CanDelete)
                throw new AccessViolationException("Can not be deleted");
            await _repository.DeleteAsync(Role);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("Id cannot be null");
            var Role = GetById(Id);
            if (Role == null)
                throw new NullReferenceException("Role cannot be null");
            if (!Role.CanDelete)
                throw new AccessViolationException("Can not be deleted");

            await _repository.DeleteAsync(Role);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public Role GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Role> GetByName(string name)
        {
            return _repository.Table.Where(x => x.Name.Contains(name)).ToList();
        }

        public Role IncludePermissions(Role role)
        {
            if (role == null) throw new NullReferenceException("role cannot be null");
            var roles = IncludePermissions(new List<Role> { role });
            if (roles != null && roles.Count > 0)
                return roles.First();
            return role;
        }
        
        public List<Role> IncludePermissions(List<Role> roles)
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

        public List<Role> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Role> GetAsQueryable()
        {
            return _repository.Table;
        }
    }
}
