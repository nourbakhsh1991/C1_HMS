using BMS.Domain.Geometry.Point;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Line
{
    public class LwPolyLineEntity : GeometryEntity
    {
        public List<PointEntity> Vertices { get; set; }
        public List<PointEntity> Centers { get; set; }
        public List<PointEntity> Angles { get; set; }
        public List<float> Radius { get; set; }
        public List<float> DashArray { get; set; }
        public bool IsClosed { get; set; }

    }
}
