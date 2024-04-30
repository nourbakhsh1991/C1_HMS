using BMS.Domain.BaseModels;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Map
{
    [BsonIgnoreExtraElements]
    public class Entity : BaseDBModel
    {
        public int? StrokeColor { get; set; }
        public int? Fill { get; set; }
        public string? LayerId { get; set; }
        public string? CapType { get; set; }
        public int? StrokeWidth { get; set; }
        public string? EntityType { get; set; }
        public object? Data { get; set; }

        public List<object>? StateData { get; set; }
        public bool? Interactive { get; set; }
        public string? OnClick { get; set; }
        public string? OnPointerDown { get; set; }
        public string? OnPointerUp { get; set; }
        public string? OnPointerOver { get; set; }
        public string? OnPointerOut { get; set; }
        public string? OnPointerMove { get; set; }
        public string? OnFrameRequested { get; set; }
    }
}
