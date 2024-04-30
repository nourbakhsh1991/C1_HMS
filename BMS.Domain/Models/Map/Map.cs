using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Extentions;
using MongoDB.Bson.Serialization.Attributes;

namespace BMS.Domain.Models.Map
{
    public class Map : BaseDBModel, IBaseModel<MMap>
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        [BsonIgnore]
        public User User { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOriginalSize { get; set; }
        public string Path { get; set; }
        [BsonIgnore]
        public List<Layer> Layers { get; set; }

        public MMap GetM()
        {
            return new MMap
            {
                Name = Name,
                Metadata = Metadata.GetDotNetObject(),
                Id = Id,
                User = User?.GetM(),
                CreatedTime = CreatedTime.Ticks,
                UpdatedTime = UpdatedTime.Ticks,
                Path = Path,
                Layers = Layers != null ? Layers.Select(a => a.GetM()).ToList() : new List<MLayer>(),
                Height = Height,
                IsOriginalSize = IsOriginalSize,
                Width = Width
            };
        }
    }
}
