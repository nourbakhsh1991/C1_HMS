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
using CSMath;
using ACadSharp.Entities;
using BMS.Domain.Models.Map;

namespace BMS.Domain.Converters
{
    public class LwPolyLineConverter : CADConverter
    {
        private List<PointF> _points = null;
        private List<PointF> _centers = null;
        private List<PointF> _angles = null;
        private List<float> _radius = null;
        private ACadSharp.Entities.LwPolyline line = null;
        private Domain.Models.Map.Layer _layer;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private List<float> _dashArray = new List<float>();
        public LwPolyLineConverter(ACadSharp.Entities.LwPolyline line, Matrix<float> transform, Vector<float> mapScale, Domain.Models.Map.Layer layer = null)
        {
            _points = new List<PointF>();
            _centers = new List<PointF>();
            _angles = new List<PointF>();
            _radius = new List<float>();
            this.line = line;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            _strokeWidth = (int)line.LineWeight;
            _strokeColor =
                        (line.Color.IsByLayer ? -1 :
                        (line.Color.IsByBlock ? -2 : (
                        line.Color.IsTrueColor ? line.Color.TrueColor :
                        line.Color.Index)));

            foreach (var segment in line.LineType.Segments)
            {
                _dashArray.Add(Math.Abs((float)segment.Length) + 1);
            }

            var minWidth = _strokeWidth;
            for (int i = 0; i < line.Vertices.Count; i++)
            {
                var _point = new PointF();

                var start = V.DenseOfArray(new float[] { (float)line.Vertices[i].Location.X, (float)line.Vertices[i].Location.Y, 1 });
                var transformedStart = transform * start;
                _point.X = new SvgUnit(transformedStart[0]);
                _point.Y = new SvgUnit(transformedStart[1]);

                _points.Add(_point);

                // Straight line
                if (Math.Abs(line.Vertices[i].Bulge) <= 0.000001)
                {
                    _centers.Add(new PointF());
                    _angles.Add(new PointF());
                    _radius.Add(0);
                }
                else
                {
                    var startVertex = line.Vertices[i].Location;
                    var endVertex = (i == line.Vertices.Count - 1) ? line.Vertices[0].Location : line.Vertices[i + 1].Location;
                    var bulge = (float)line.Vertices[i].Bulge;

                    var theta = Math.Atan(Math.Abs(bulge)) * 4;
                    var gamma = (Math.PI - theta) / 2;


                    var flip = MathF.Sign(transform[0, 0]) * MathF.Sign(transform[1, 1]) < 0;

                    var angle = flip ? Math.Atan2((endVertex.Y - startVertex.Y), (endVertex.X - startVertex.X))
                        : Math.Atan2((startVertex.Y - endVertex.Y), (startVertex.X - endVertex.X));

                    if (bulge >= 0) angle += gamma;
                    else angle -= gamma;


                    var midpoint = (startVertex + endVertex) / 2;
                    var direction = (endVertex - startVertex).Normalize();
                    var normal = new CSMath.XY(-direction.Y, direction.X); //new Vector2(-direction.Y, direction.X); // CCW perpendicular

                    if (bulge < 0)
                    {
                        normal = -normal;  // Flip the normal for clockwise direction
                    }

                    var distance = startVertex.DistanceFrom(endVertex);
                    var s = (distance / (2 * -Math.Abs(bulge)));
                    var radius = ((distance / 2) * (distance / 2) + s * s) / (2 * s);

                    var cx = startVertex.X + radius * Math.Cos(angle);
                    var cy = startVertex.Y + radius * Math.Sin(angle);

                    var center = midpoint + normal * (s);
                    //var center = new CSMath.XY(cx, cy);
                    var centerV = V.DenseOfArray(new float[] { (float)center.X, (float)center.Y, 1 });
                    var transformedCenter = transform * centerV;

                    var localTransformWithoutTraslation = M.DenseOfArray(transform.ToArray());
                    localTransformWithoutTraslation[0, 2] = 0;
                    localTransformWithoutTraslation[1, 2] = 0;


                    var radiusV = V.DenseOfArray(new float[] { (float)(((radius))), (float)(((radius))), 1 });
                    var transformedRadius = localTransformWithoutTraslation * radiusV;
                    var radiusX = MathF.Abs(transformedRadius[0]);



                    var startV = V.DenseOfArray(new float[] { (float)startVertex.X, (float)startVertex.Y, 1 });
                    var endV = V.DenseOfArray(new float[] { (float)endVertex.X, (float)endVertex.Y, 1 });
                    var trasnformStart = transform * startV;
                    var trasnformEnd = transform * endV;

                    var angleStart = MathF.Atan2((trasnformStart[1] - transformedCenter[1]), (trasnformStart[0] - transformedCenter[0]));
                    var angleEnd = MathF.Atan2((trasnformEnd[1] - transformedCenter[1]), (trasnformEnd[0] - transformedCenter[0]));

                    var _startAngle = flip ? angleStart : angleEnd; // (float)arc.StartAngle + rotation;
                    var _endAngle = flip ? angleEnd : angleStart;// (float)arc.EndAngle + rotation;

                    _centers.Add(new PointF((float)transformedCenter[0], (float)transformedCenter[1]));
                    _angles.Add(new PointF((float)_startAngle, (float)_endAngle));
                    _radius.Add(Math.Sign(line.Vertices[i].Bulge) * (float)radiusX);
                }

                //if (i > 0)
                //{
                //    var angle = MathF.Atan2((_points[i].Y - _points[i - 1].Y), (_points[i].X - _points[i - 1].X));
                //    var sclaedWidth = MathF.Abs(MathF.Cos(angle) * mapScale[1] * _strokeWidth) + MathF.Abs(MathF.Sin(angle) * mapScale[0] * _strokeWidth);
                //    if (sclaedWidth < minWidth)
                //        minWidth = sclaedWidth;
                //}

            }
            //_strokeWidth = minWidth;

            //if (line.IsClosed)
            //{
            //    var _point = new SvgPoint();
            //    var start = V.DenseOfArray(new float[] { (float)line.Vertices[line.Vertices.Count - 1].Location.X, (float)line.Vertices[line.Vertices.Count - 1].Location.Y, 1 });
            //    var transformedStart = transform * start;
            //    _point.X = new SvgUnit(transformedStart[0]);
            //    _point.Y = new SvgUnit(transformedStart[1]);
            //    _points.Add(_point);
            //}
        }


        public List<PointF> Get()
        {
            return _points;
        }
        public List<PointF> GetAngles()
        {
            return _angles;
        }

        public List<PointF> GetCenters()
        {
            return _centers;
        }

        public List<float> GetRadious()
        {
            return _radius;
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
