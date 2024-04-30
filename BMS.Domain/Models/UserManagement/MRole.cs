using MongoDB.Bson.Serialization.Attributes;
using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.UserManagement
{
    public class MRole : MBaseDBModel, IMModel<Role>
    {
        // Specifey the name 
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Permission full description
        [StringLength(1000)]
        public string Description { get; set; }

        // array of all permissions
        public List<MPermission> Permissions { get; set; }


        // is it main Permission and can't be deleted or not
        [Required]
        public bool CanDelete { get; set; } = true;

        // is it main Permission and can't be edited or not
        [Required]
        public bool CanEdit { get; set; } = true;

        public long CreatedTime { get; set; }

        public long UpdatedTime { get; set; }

        public Role GetBase()
        {
            return new Role
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CanDelete = CanDelete,
                CanEdit = CanEdit,
                CreatedTime = CreatedTime != 0 ? new DateTime(CreatedTime) : new DateTime(),
                UpdatedTime = UpdatedTime != 0 ? new DateTime(UpdatedTime) : new DateTime(),
                Metadata = Metadata.GetBsonObject(),
                PermissionIds = Permissions == null ? new List<string>() : Permissions.Select(a => a.Id).ToList(),
                Permissions = Permissions == null ? new List<Permission>() : Permissions.Select(a => a.GetBase()).ToList()
            };
        }
    }
}
