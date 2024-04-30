using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Permissions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        IRepository<Permission> _repository;

        public PermissionService(IRepository<Permission> repository)
        {
            _repository = repository;
        }

        public async Task Insert(Permission permission)
        {
            if (permission == null)
                throw new Exception();
            permission.Id = null;
            permission.CreatedTime = DateTime.Now;
            permission.UpdatedTime = DateTime.Now;
            await _repository.InsertAsync(permission);
        }

        public async Task Insert(List<Permission> permissions)
        {
            if (permissions == null)
                throw new Exception();
            await _repository.InsertAsync(permissions);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public async Task Update(Permission permission)
        {
            if (permission == null)
                throw new NullReferenceException("permission cannot be null");
            if (!permission.CanEdit)
                throw new AccessViolationException("Can not be edited");
            permission.UpdatedTime = DateTime.Now;
            await _repository.UpdateAsync(permission);
        }

        public async Task Delete(Permission permission)
        {
            if (permission == null)
                throw new NullReferenceException("permission cannot be null");

            if (!permission.CanDelete)
                throw new AccessViolationException("Can not be deleted");
            await _repository.DeleteAsync(permission);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("Id cannot be null");
            var permission = GetById(Id);
            if (permission == null)
                throw new NullReferenceException("permission cannot be null");
            if (!permission.CanDelete)
                throw new AccessViolationException("Can not be deleted");

            await _repository.DeleteAsync(permission);
        }

        public Permission GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Permission> GetByName(string name)
        {
            return _repository.Table.Where(x => x.Name.Contains(name)).ToList();
        }

        public List<Permission> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Permission> GetAsQueryable()
        {
            return _repository.Table;
        }
    }
}
