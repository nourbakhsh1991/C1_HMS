using BMS.Domain.Interfaces;
using BMS.Domain.Menues;
using BMS.Services.Menus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Menus
{
    public class MenuService : IMenuService
    {
        IRepository<AsideMenuItem> repository;

        public MenuService(
            IRepository<AsideMenuItem> repository)
        {
            this.repository = repository;
        }


        public async Task Insert(AsideMenuItem menu)
        {
            if (menu == null)
                throw new Exception();
            await repository.InsertAsync(menu);
        }

        public async Task Update(AsideMenuItem menu)
        {
            if (menu == null)
                throw new Exception();
            await repository.UpdateAsync(menu);
        }

        public async Task Delete(AsideMenuItem menu)
        {
            if (menu == null)
                throw new NullReferenceException("AsideMenuItem cannot be null");

            await repository.DeleteAsync(menu);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("id cannot be null");
            var menu = GetById(Id);
            if (menu == null)
                throw new NullReferenceException("AsideMenuItem cannot be null");
            await repository.DeleteAsync(menu);
        }

        public async Task InsertSunMenu(AsideMenuItem menu, AsideMenuItem sub)
        {
            if (menu == null || sub == null)
                throw new Exception();
            menu.SubMenuIds.Add(sub.Id);
            menu.SubMenus.Add(sub);
            await Update(menu);
        }

        public async Task InsertSunMenu(AsideMenuItem menu, string subId)
        {
            if (menu == null)
                throw new Exception();
            menu.SubMenuIds.Add(subId);
            await Update(menu);
        }

        public async Task RemoveSunMenu(AsideMenuItem menu, AsideMenuItem sub)
        {
            if (menu == null || sub == null)
                throw new Exception();
            menu.SubMenuIds.Remove(sub.Id);
            menu.SubMenus.Remove(sub);
            await Update(menu);
        }

        public async Task RemoveSunMenu(AsideMenuItem menu, string subId)
        {
            if (menu == null)
                throw new Exception();
            menu.SubMenuIds.Remove(subId);
            await Update(menu);
        }

        private List<AsideMenuItem> GetAllSubmenus(AsideMenuItem menu)
        {
            if (!menu.HasSubMenu) return new List<AsideMenuItem> { };
            var menus = repository.Table.Where(a => menu.SubMenuIds.Contains(a.Id)).ToList();
            foreach (var itm in menus)
            {
                itm.SubMenus = GetAllSubmenus(itm);
                menu.SubMenus.Add(itm);
            }
            return menus;
        }

        public AsideMenuItem GetById(string id)
        {
            AsideMenuItem itm = repository.GetById(id);
            if (itm == null) return null;
            var subMenus = GetAllSubmenus(itm);
            itm.SubMenus = subMenus;
            return itm;
        }

        public List<AsideMenuItem> GetByName(string name)
        {
            return GetAsQueryable().Where(x => x.Name.Contains(name)).ToList();
        }

        public List<AsideMenuItem> GetAll()
        {
            return repository.GetAllAsync().Result;
        }

        public IQueryable<AsideMenuItem> GetAsQueryable()
        {
            return repository.Table;
        }
    }
}
