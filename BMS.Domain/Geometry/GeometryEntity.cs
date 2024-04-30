using BMS.Domain.BaseModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace BMS.Domain.Geometry
{
    public class GeometryEntity
    {
        public int? StrokeColor { get; set; }
        public string? LayerId { get; set; }
        public string? CapType { get; set; }

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public int? StrokeWidth { get; set; }
        public int? Fill { get; set; }
        public string? Type { get; set; }
        //public Matrix<float> Transform { get; set; }

        [BsonExtraElements]
        public BsonDocument? ExtraElements { get; set; }
    }
}
