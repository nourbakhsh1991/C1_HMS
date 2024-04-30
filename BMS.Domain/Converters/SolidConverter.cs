using ACadSharp;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using Svg.Pathing;
using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BMS.Domain.Converters
{
    public class SolidConverter : CADConverter
    {
        private List<PointF> _points = null;
        private ACadSharp.Entities.Solid solid = null;
        private Domain.Models.Map.Layer _layer;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private List<float> _dashArray = new List<float>();
        public SolidConverter(ACadSharp.Entities.Solid solid, Matrix<float> transform, Vector<float> mapScale, Domain.Models.Map.Layer layer = null)
        {
            _points = new List<PointF>();
            this.solid = solid;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            _strokeWidth = (int)solid.LineWeight;
            _strokeColor =
                        (solid.Color.IsByLayer ? -1 :
                        (solid.Color.IsByBlock ? -2 : (
                        solid.Color.IsTrueColor ? solid.Color.TrueColor :
                        solid.Color.Index)));

            foreach (var segment in solid.LineType.Segments)
            {
                _dashArray.Add(Math.Abs((float)segment.Length) + 1);
            }
            var p1 = solid.FirstCorner;
            CSMath.XYZ p2, p3 , p4;
            var p22 = solid.SecondCorner;
            var p33 = solid.ThirdCorner;
            var p44 = solid.FourthCorner;
            if ((isLeft(p1, p22, p33) && isRight(p1, p22, p44)) ||
                (isRight(p1, p22, p33) && isLeft(p1, p22, p44)))
            {
                p2 = p33;
                p3 = p22;
                p4 = p44;
            }
            else if ((isLeft(p1, p33, p22) && isRight(p1, p33, p44)) ||
                (isRight(p1, p33, p22) && isLeft(p1, p33, p44)))
            {
                p2 = p22;
                p3 = p33;
                p4 = p44;
            }
            else if ((isLeft(p1, p44, p33) && isRight(p1, p44, p22)) ||
                (isRight(p1, p44, p33) && isLeft(p1, p44, p22)))
            {
                p2 = p22;
                p3 = p44;
                p4 = p33;
            }
            else
            {
                p2 = p22;
                p3 = p33; 
                p4 = p44;
            }

            var points = new List<CSMath.XYZ>() { p1,p2,p3,p4};
            for (int i = 0; i < points.Count; i++)
            {
                var _point = new PointF();

                var start = V.DenseOfArray(new float[] { (float)points[i].X, (float)points[i].Y, 1 });
                var transformedStart = transform * start;
                _point.X = new SvgUnit(transformedStart[0]);
                _point.Y = new SvgUnit(transformedStart[1]);

                _points.Add(_point);
                
            }
        }

        private bool isLeft(CSMath.XYZ a, CSMath.XYZ b, CSMath.XYZ c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X) >= 0;
        }

        private bool isRight(CSMath.XYZ a, CSMath.XYZ b, CSMath.XYZ c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X) <= 0;
        }


        public List<PointF> Get()
        {
            return _points;
        }

        public int GetStrokeWidth()
        {
            return _strokeWidth;
        }

        public int GetStrokeColor()
        {
            return _strokeColor;
        }

        public double GetLength()
        {
            float sum = 0;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                var StartX = _points[i].X;
                var EndX = _points[i + 1].X;
                var StartY = _points[i].Y;
                var EndY = _points[i + 1].Y;
                sum += MathF.Sqrt((StartX - EndX) * (StartX - EndX)
                + (StartY - EndY) * (StartY - EndY));
            }

            return sum;
        }

        public List<float> GetDashArray()
        {
            return _dashArray;
        }

        public SvgPathSegmentList GetSegments()
        {
            var res = new SvgPathSegmentList();
            var svgMove = new SvgMoveToSegment(false, new System.Drawing.PointF(_points[0].X, _points[0].Y));
            res.Add(svgMove);
            foreach (var _point in _points)
            {
                var svgLine = new SvgLineSegment(false, new System.Drawing.PointF(_point.X, _point.Y));
                res.Add(svgLine);
            }
            return res;
        }

    }
}
