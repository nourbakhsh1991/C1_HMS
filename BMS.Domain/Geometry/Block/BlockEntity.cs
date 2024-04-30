using BMS.Domain.Geometry.Point;
using BMS.Domain.Models.Map;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Block
{
    public class BlockEntity : GeometryEntity
    {
        public List<Entity> Entities { get; set; }

        public byte[] Preview { get; set; }

        public string Name { get; set; }


    }
}
