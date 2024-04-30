using BMS.Domain.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Roles.Interfaces
{
    public interface IRoleService
    {
        Task Delete(Role Role);
        Task Delete(string Id);
        Task DeleteAll();
        List<Role> GetAll();
        IQueryable<Role> GetAsQueryable();
        Role GetById(string id);
        List<Role> GetByName(string name);
        Role IncludePermissions(Role role);
        List<Role> IncludePermissions(List<Role> roles);
        Task Insert(Role Role);
        Task Insert(List<Role> Roles);
        Task Update(Role Role);
    }
}
