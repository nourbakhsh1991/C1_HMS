using BMS.Domain.Geometry.Point;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Line
{
    public class LineEntity : GeometryEntity
    {
        public PointEntity Start { get; set; }
        public PointEntity End { get; set; }
        public List<float> DashArray { get; set; }
    }
}
