using MathNet.Numerics.LinearAlgebra;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Line
{
    public static class LineEntityExtensions
    {
        public static void UpdateTransform(this LineEntity line , Matrix<float> transform)
        {
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var start = V.DenseOfArray(new float[] { (float)line.Start.X, (float)line.Start.Y, 1 });
            var end = V.DenseOfArray(new float[] { (float)line.End.X, (float)line.End.Y, 1 });
            var transformedStart = transform * start;
            var transformedEnd = transform * end;
            line.Start.X = transformedStart[0];
            line.Start.Y = transformedStart[1];
            line.End.X = transformedEnd[0];
            line.End.Y = transformedEnd[1];
        }

        public static void UpdateTransform(this LwPolyLineEntity lwpolyline, Matrix<float> transform)
        {
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            for (int i = 0; i < lwpolyline.Vertices.Count; i++)
            {
                var _point = new PointF();

                var start = V.DenseOfArray(new float[] { (float)lwpolyline.Vertices[i].X, (float)lwpolyline.Vertices[i].Y, 1 });
                var transformedStart = transform * start;
                _point.X = new SvgUnit(transformedStart[0]);
                _point.Y = new SvgUnit(transformedStart[1]);
                lwpolyline.Vertices[i].X = _point.X;
                lwpolyline.Vertices[i].Y = _point.Y;
            }
        }
    }
}
