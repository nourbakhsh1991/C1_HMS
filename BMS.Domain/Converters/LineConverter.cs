using ACadSharp;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Converters
{
    public class LineConverter : CADConverter
    {
        private ACadSharp.Entities.Line line = null;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private float _length = 0;
        private Domain.Models.Map.Layer _layer;
        private List<float> _dashArray = new List<float>();
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float EndX { get; set; }
        public float EndY { get; set; }
        public LineConverter(ACadSharp.Entities.Line line, Matrix<float> transform, Domain.Models.Map.Layer layer = null)
        {
            this.line = line;
            _layer = layer;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var start = V.DenseOfArray(new float[] { (float)line.StartPoint.X, (float)line.StartPoint.Y, 1 });
            var end = V.DenseOfArray(new float[] { (float)line.EndPoint.X, (float)line.EndPoint.Y, 1 });
            var transformedStart = transform * start;
            var transformedEnd = transform * end;
            StartX = transformedStart[0];
            StartY = transformedStart[1];
            EndX = transformedEnd[0];
            EndY = transformedEnd[1];
            foreach (var segment in line.LineType.Segments)
            {
                _dashArray.Add(Math.Abs((float)segment.Length) + 1);
            }
            var angle = MathF.Atan2((transformedEnd[1] - transformedStart[1]), (transformedEnd[0] - transformedStart[0]));
            _length = MathF.Sqrt((transformedStart[0] - transformedEnd[0]) * (transformedStart[0] - transformedEnd[0])
                + (transformedStart[1] - transformedEnd[1]) * (transformedStart[1] - transformedEnd[1]));
            
            _strokeWidth = (int)line.LineWeight;
            _strokeColor = 
                        (line.Color.IsByLayer ? -1 :
                        (line.Color.IsByBlock ? -2 : (
                        line.Color.IsTrueColor ? line.Color.TrueColor :
                        line.Color.Index)));

        }

        public int GetStrokeWidth()
        {
            return _strokeWidth;
        }

        public int GetStrokeColor()
        {
            return _strokeColor;
        }

        public float GetLength()
        {
            return _length;
        }

        public List<float> GetSegments()
        {
            return _dashArray;
        }

    }
}
