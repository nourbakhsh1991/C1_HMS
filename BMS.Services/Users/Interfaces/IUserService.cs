using BMS.Domain.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Users.Interfaces
{
    public interface IUserService
    {
        Task Delete(User user);
        Task Delete(string Id);
        Task DeleteAll();
        List<User> GetAll();
        IQueryable<User> GetAsQueryable();
        User GetById(string id);
        User GetByUsername(string name);
        List<Permission> GetPermissionsByClaims(ClaimsPrincipal user);
        User IncludeRoles(User user);
        List<User> IncludeRoles(List<User> users);
        Task Insert(User user);
        Task Insert(List<User> users);
        Task Update(User user);
        Task UpdateLogin(User user);
    }
}
