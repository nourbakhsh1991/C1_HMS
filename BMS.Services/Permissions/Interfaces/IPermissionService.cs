using BMS.Domain.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Permissions.Interfaces
{
    public interface IPermissionService
    {
        Task Delete(Permission permission);
        Task Delete(string Id);
        Task DeleteAll();
        List<Permission> GetAll();
        IQueryable<Permission> GetAsQueryable();
        Permission GetById(string id);
        List<Permission> GetByName(string name);
        Task Insert(Permission permission);
        Task Insert(List<Permission> permissions);
        Task Update(Permission permission);
    }
}
