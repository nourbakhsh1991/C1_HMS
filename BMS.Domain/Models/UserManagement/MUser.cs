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
    public class MUser : MBaseDBModel, IMModel<User>
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

        // array of all roles
        [BsonIgnore]
        public List<MRole> Roles { get; set; }

        [Required]
        public bool CanDelete { get; set; } = true;

        [Required]
        public bool CanEdit { get; set; } = true;

        public long CreatedTime { get; set; }

        public long LastLogin { get; set; }

        public User GetBase()
        {
            return new User
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                CanDelete = CanDelete,
                CanEdit = CanEdit,
                CreatedTime = CreatedTime != 0 ? new DateTime(CreatedTime) : new DateTime(),
                LastLogin = LastLogin != 0 ? new DateTime(LastLogin) : new DateTime(),
                Metadata = Metadata.GetBsonObject(),
                Roles = Roles == null ? new List<Role>() : Roles.Select(x => x.GetBase()).ToList(),
                RoleIds = Roles == null ? new List<string>() : Roles.Select(x => x.Id).ToList(),
                Email = Email,
                Password = Password,
                Username = Username
            };
        }
    }
}
