using BMS.Domain.Geometry.Point;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Text
{
    public class MTextEntity : GeometryEntity
    {
        public PointEntity InsertPoint { get; set; }
        public PointEntity Size { get; set; }
        public string Text {get; set; }

    }
}
