using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Point
{
    public class PointEntity : GeometryEntity
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
