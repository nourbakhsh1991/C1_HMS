using BMS.Domain.BaseModels;
using BMS.Domain.Geometry;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Extentions;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Map
{
    public class Layer : BaseDBModel, IBaseModel<MLayer>
    {
        public string? Name { get; set; }
        public string? LayerName { get; set; }
        public string? Description { get; set; }
        public int? StrokeColor { get; set; }
        public int? StrokeWidth { get; set; }
        public string? MapId { get; set; }
        [BsonIgnore]
        public Domain.Models.Map.Map Map { get; set; }
        public string? UserId { get; set; }
        [BsonIgnore]
        public User User { get; set; }
        public List<string> PermissionIds { get; set; }
        [BsonIgnore]
        public List<Permission> Permissions { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        [BsonIgnore]
        public List<Entity> Entities { get; set; }
        public bool DefaultPreview { get; set; }
        public bool Include { get; set; }
        public Layer()
        {
            PermissionIds = new List<string>();
            Permissions = new List<Permission>();
            CreatedTime = new DateTime();
            UpdatedTime = new DateTime();
        }
        public MLayer GetM()
        {
            return new MLayer
            {
                Name = Name,
                Metadata = Metadata.GetDotNetObject(),
                Id = Id,
                User = User?.GetM(),
                CreatedTime = CreatedTime.Ticks,
                UpdatedTime = UpdatedTime.Ticks,
                StrokeColor = StrokeColor,
                Description= Description,
                MapId= MapId,
                Permissions = Permissions != null ? 
                                    Permissions.Select(a=>a.GetM()).ToList() :
                                    new List<MPermission>(),
                StrokeWidth = StrokeWidth,
                LayerName = LayerName,
                Map = Map?.GetM(),
                DefaultPreview = DefaultPreview,
                Include = Include,
                Entities = Entities,
            };
        }
    }
}
