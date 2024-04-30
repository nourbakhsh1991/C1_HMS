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
    public class Permission : BaseDBModel, IBaseModel<MPermission>
    {
        // Specifey the name 
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Permission full description
        [StringLength(1000)]
        public string Description { get; set; }


        // is it main Permission and can't be deleted or not
        [Required]
        public bool CanDelete { get; set; } = true;

        // is it main Permission and can't be edited or not
        [Required]
        public bool CanEdit { get; set; } = true;

        public DateTime? CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public MPermission GetM()
        {
            return new MPermission
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CanDelete = CanDelete,
                CanEdit = CanEdit,
                CreatedTime = CreatedTime.HasValue ? CreatedTime.Value.Ticks : 0,
                UpdatedTime = CreatedTime.HasValue ? CreatedTime.Value.Ticks : 0,
                Metadata = Metadata.GetDotNetObject(),
            };
        }
    }
}
