using BMS.Domain.Menues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Menus.Interfaces
{
    public interface IMenuService
    {
        Task Delete(AsideMenuItem menu);
        Task Delete(string Id);
        List<AsideMenuItem> GetAll();
        IQueryable<AsideMenuItem> GetAsQueryable();
        AsideMenuItem GetById(string id);
        List<AsideMenuItem> GetByName(string name);
        Task Insert(AsideMenuItem menu);
        Task InsertSunMenu(AsideMenuItem menu, AsideMenuItem sub);
        Task InsertSunMenu(AsideMenuItem menu, string subId);
        Task RemoveSunMenu(AsideMenuItem menu, AsideMenuItem sub);
        Task RemoveSunMenu(AsideMenuItem menu, string subId);
        Task Update(AsideMenuItem menu);
    }
}
