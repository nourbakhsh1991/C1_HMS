using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Extentions;

namespace BMS.Domain.Models.Map
{
    public class MMap : MBaseDBModel, IMModel<Map>
    {
        public string Name { get; set; }
        public MUser User { get; set; }
        public long CreatedTime { get; set; }
        public long UpdatedTime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOriginalSize { get; set; }
        public string Path { get; set; }
        public List<MLayer> Layers { get; set; }
        public Map GetBase()
        {
            return new Map
            {
                Name = Name,
                User = User?.GetBase(),
                CreatedTime = CreatedTime != -1 ? new DateTime(CreatedTime) : new DateTime(),
                UpdatedTime = UpdatedTime != -1 ? new DateTime(UpdatedTime) : new DateTime(),
                Id = Id,
                UserId = User != null ? User.Id : null,
                Metadata = Metadata.GetBsonObject(),
                Path = Path,
                Layers = Layers != null ? Layers.Select(a => a.GetBase()).ToList() : new List<Layer>(),
                Height = Height,
                IsOriginalSize = IsOriginalSize,
                Width = Width
            };
        }
    }
}
