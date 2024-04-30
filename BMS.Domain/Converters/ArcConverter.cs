using ACadSharp;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using Svg;
using Svg.Pathing;

namespace BMS.Domain.Converters
{
    public class ArcConverter : CADConverter
    {
        private ACadSharp.Entities.Arc arc = null;
        private System.Drawing.PointF _center;
        private System.Drawing.PointF _start;
        private System.Drawing.PointF _end;
        private float _startAngle;
        private float _endAngle;
        private float _radius;
        private Domain.Models.Map.Layer _layer; 
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private float _length = 0;

        public ArcConverter(ACadSharp.Entities.Arc arc, Matrix<float> transform, Vector<float> mapScale, float rotation = 0, Domain.Models.Map.Layer layer = null)
        {
            _layer = layer;
            this.arc = arc;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            
            var startX = (arc.Center.X) + arc.Radius * Math.Cos(arc.StartAngle);
            var startY = (arc.Center.Y) + arc.Radius * Math.Sin(arc.StartAngle);

            var endX = (arc.Center.X) + arc.Radius * Math.Cos(arc.EndAngle);
            var endY = (arc.Center.Y) + arc.Radius * Math.Sin(arc.EndAngle);

            var sA = arc.StartAngle;

            var start = V.DenseOfArray(new float[] { (float)startX, (float)startY, 1 });
            var end = V.DenseOfArray(new float[] { (float)endX, (float)endY, 1 });
            var center = V.DenseOfArray(new float[] { (float)((arc.Center.X)), (float)((arc.Center.Y)), 1 });

            var transformLocal = transform;
            var transformedStart = transformLocal * start;
            var transformedEnd = transformLocal * end;
            var transformedCenter = transformLocal * center;

            var flip = MathF.Sign(transformLocal[0, 0]) * MathF.Sign(transformLocal[1, 1]) < 0;


            _start = new System.Drawing.PointF(transformedStart[0], transformedStart[1]);
            _end = new System.Drawing.PointF(transformedEnd[0], transformedEnd[1]);
            _center = new System.Drawing.PointF(transformedCenter[0], transformedCenter[1]);
            _radius = MathF.Sqrt((_center.X - _start.X)*(_center.X - _start.X) +
                                        (_center.Y - _start.Y) * (_center.Y - _start.Y));

            var minRatio = MathF.Min(mapScale[0], mapScale[1]);

            _strokeWidth = (int)arc.LineWeight;
            _strokeColor = (arc.Color.IsByLayer ? -1 :
                        (arc.Color.IsByBlock ? -2 : (
                        arc.Color.IsTrueColor ? arc.Color.TrueColor :
                        arc.Color.Index)));

            if(_strokeColor == 3)
            {
                var a = 0;
            }

            //_strokeWidth = _strokeWidth * minRatio;

            //var angleStart = MathF.Atan2((transformedCenter[1] - transformedStart[1]), (transformedCenter[0] - transformedStart[0]));
            //var angleEnd = MathF.Atan2((transformedCenter[1] - transformedEnd[1]), (transformedCenter[0] - transformedEnd[0]));

            var angleStart = MathF.Atan2((transformedStart[1] - transformedCenter[1]), (transformedStart[0] - transformedCenter[0]));
            var angleEnd = MathF.Atan2((transformedEnd[1] - transformedCenter[1]), (transformedEnd[0] - transformedCenter[0]));

            _startAngle = flip ? angleStart : angleEnd; // (float)arc.StartAngle + rotation;
            _endAngle = flip ? angleEnd : angleStart;// (float)arc.EndAngle + rotation;
        }


        public float GetRadius()
        {
            return _radius;
        }

        public System.Drawing.PointF GetCenter()
        {
            return _center;
        }

        public System.Drawing.PointF GetStart()
        {
            return _start;
        }

        public System.Drawing.PointF GetEnd()
        {
            return _end;
        }

        public float GetStartAngle()
        {
            return _startAngle;
        }

        public float GetEndAngle()
        {
            return _endAngle;
        }

        public int GetStrokeWidth()
        {
            return _strokeWidth;
        }

        public int GetStrokeColor()
        {
            return _strokeColor;
        }

        public List<float> GetSegments()
        {
            return new List<float>();
        }

    }
}
