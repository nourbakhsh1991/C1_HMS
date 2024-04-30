using BMS.Domain.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Map
{
    public class State
    {
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
        public float Rotation { get; set; }
        public string BlockId { get; set; }
        public string ObjectId { get; set; }
        public SizeEntity Bound { get; set; }
    }
}
