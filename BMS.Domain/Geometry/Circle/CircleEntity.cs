using BMS.Domain.Geometry.Point;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Circle
{
    public class CircleEntity : GeometryEntity
    {
        public PointEntity Center { get; set; }
        public float Radius { get; set; }
    }
}
