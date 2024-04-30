using BMS.Domain.Geometry.Line;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Arc
{
    public static class ArcEntityExtensions
    {
        public static void UpdateTransform(this ArcEntity arc, Matrix<float> transform, bool flip = false)
        {
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


            arc.Center.X = transformedCenter[0];
            arc.Center.Y = transformedCenter[1];


            var _start = new System.Drawing.PointF(transformedStart[0], transformedStart[1]);
            var _end = new System.Drawing.PointF(transformedEnd[0], transformedEnd[1]);
            var _center = new System.Drawing.PointF(transformedCenter[0], transformedCenter[1]);
            var _radius = MathF.Sqrt((_center.X - _start.X) * (_center.X - _start.X) +
                                        (_center.Y - _start.Y) * (_center.Y - _start.Y));
            arc.Radius = _radius;

            var angleStart = MathF.Atan2((transformedStart[1] - transformedCenter[1]), (transformedStart[0] - transformedCenter[0]));
            var angleEnd = MathF.Atan2((transformedEnd[1] - transformedCenter[1]), (transformedEnd[0] - transformedCenter[0]));


            arc.StartAngle = flip ? angleStart : angleEnd;
            arc.EndAngle = flip ? angleEnd : angleStart;
        }
    }
}
