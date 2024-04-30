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
    public class User : BaseDBModel, IBaseModel<MUser>
    {
        // Specifey the name 
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [StringLength(25, MinimumLength = 5)]
        public string? Password { get; set; }


        // array of all role Ids
        public List<string> RoleIds { get; set; } = new List<string>();

        // array of all roles
        [BsonIgnore]
        public List<Role> Roles { get; set; }

        [Required]
        public bool CanDelete { get; set; } = true;

        [Required]
        public bool CanEdit { get; set; } = true;

        public DateTime? CreatedTime { get; set; }

        public DateTime? LastLogin { get; set; }

        public MUser GetM()
        {
            return new MUser
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = Password,
                CanDelete = CanDelete,
                CanEdit = CanEdit,
                CreatedTime = CreatedTime.HasValue ? CreatedTime.Value.Ticks : 0,
                LastLogin = LastLogin.HasValue ? LastLogin.Value.Ticks : 0,
                Metadata = Metadata.GetDotNetObject(),
                Roles = Roles == null ? new List<MRole>() : Roles.Select(x => x.GetM()).ToList(),
                Email = Email,
            };
        }
    }
}
