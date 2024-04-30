using BMS.Domain.BaseModels;
using BMS.Shared.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Menues
{
    public class MAsideMenuItem : MBaseDBModel
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public string? RouterLinkActive { get; set; }
        public string? RequiredPermission { get; set; }
        public bool IsSeparator { get; set; }
        public bool HasSubMenu { get; set; }

        public List<MAsideMenuItem> SubMenu { get; set; } = new List<MAsideMenuItem> { };

        public AsideMenuItem GetAsideMenuItem()
        {
            return new AsideMenuItem
            {
                Name = Name,
                Icon = Icon,
                RouterLink = RouterLink,
                RouterLinkActive = RouterLinkActive,
                RequiredPermission = RequiredPermission,
                HasSubMenu = HasSubMenu,
                SubMenuIds = SubMenu.Select(a => a.GetAsideMenuItem()).Select(a => a.Id).ToList(),
                SubMenus = SubMenu.Select(a => a.GetAsideMenuItem()).ToList(),
                Id = Id,
                IsSeparator = IsSeparator,
                Metadata = Metadata.GetDotNetObject(),
            };
        }
    }
}
