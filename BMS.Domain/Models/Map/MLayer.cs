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
    public class MLayer : MBaseDBModel, IMModel<Layer>
    {
        public string? Name { get; set; }
        public string? LayerName { get; set; }
        public string? Description { get; set; }
        public int? StrokeColor { get; set; }
        public int? StrokeWidth { get; set; }
        public string? MapId { get; set; }
        public Domain.Models.Map.MMap? Map { get; set; }
        public MUser? User { get; set; }
        public List<MPermission> Permissions { get; set; }
        public long CreatedTime { get; set; }
        public long UpdatedTime { get; set; }
        public bool DefaultPreview { get; set; }
        [BsonIgnore]
        public List<Entity> Entities { get; set; }
        public bool Include { get; set; }
        public MLayer()
        {
            Permissions = new List<MPermission>();
            CreatedTime = -1;
            UpdatedTime = -1;
        }
        public Layer GetBase()
        {
            return new Layer
            {
                Name = Name,
                User = User?.GetBase(),
                CreatedTime = CreatedTime != -1 ? new DateTime(CreatedTime) : new DateTime(),
                UpdatedTime = UpdatedTime != -1 ? new DateTime(UpdatedTime) : new DateTime(),
                Id = Id,
                UserId = User?.Id,
                Metadata = Metadata.GetBsonObject(),
                StrokeColor = StrokeColor,
                Description = Description,
                MapId = MapId,
                Permissions = Permissions != null ?
                                    Permissions.Select(a => a.GetBase()).ToList() :
                                    new List<Permission>(),
                PermissionIds = Permissions != null ?
                                    Permissions.Select(a => a.Id).ToList() :
                                    new List<string>(),
                StrokeWidth = StrokeWidth,
                LayerName= LayerName,
                Map = Map?.GetBase(),
                DefaultPreview = DefaultPreview,
                Include = Include,
                Entities = Entities,
            };
        }
    }
}
