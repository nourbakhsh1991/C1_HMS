using MongoDB.Bson.Serialization.Attributes;
using BMS.Domain.BaseModels;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Menues
{
    public class AsideMenuItem : BaseDBModel
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public string? RouterLinkActive { get; set; }
        public string? RequiredPermission { get; set; }
        public bool IsSeparator { get; set; }
        public bool HasSubMenu { get; set; }

        public List<string> PermissionNames { get; set; }

        public List<string> SubMenuIds { get; set; } = new List<string> { };

        [BsonIgnore]
        public List<AsideMenuItem> SubMenus { get; set; } = new List<AsideMenuItem> { };

        public MAsideMenuItem GetM()
        {
            return new MAsideMenuItem()
            {
                HasSubMenu = HasSubMenu,
                Icon = Icon,
                RouterLink = RouterLink,
                RouterLinkActive = RouterLinkActive,
                RequiredPermission = RequiredPermission,
                Id = Id,
                IsSeparator = IsSeparator,
                Metadata = Metadata.GetBsonObject(),
                Name = Name,
                SubMenu = SubMenus.Select(a => a.GetM()).ToList()
            };
        }

    }
}
