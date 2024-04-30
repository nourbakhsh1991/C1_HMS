using BMS.Domain.BaseModels;
using BMS.Domain.Geometry;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Map
{
    [BsonIgnoreExtraElements]
    public class Block : BaseDBModel
    {
        public int StrokeColor { get; set; }
        public int Fill { get; set; }
        public string Name { get; set; }
        public string MapId { get; set; }
        public string CapType { get; set; }
        public int StrokeWidth { get; set; }
        public string EntityType { get; set; }
        public object Data { get; set; }
    }
}
