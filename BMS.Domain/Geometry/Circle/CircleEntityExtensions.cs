using BMS.Domain.Geometry.Line;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Circle
{
    public static class CircleEntityExtensions
    {
        public static void UpdateTransform(this CircleEntity circle, Matrix<float> transform)
        {
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var transformLocal = transform;
            var radius = V.DenseOfArray(new float[] { (float)((circle.Radius)), (float)((circle.Radius)), 1 });
            var localTransformWithoutTraslation = M.DenseOfArray(transformLocal.ToArray());
            localTransformWithoutTraslation[0, 2] = 0;
            localTransformWithoutTraslation[1, 2] = 0;

            var transformedRadius = localTransformWithoutTraslation * radius;
            var radiusX = transformedRadius[0];
            var radiusY = transformedRadius[1];

            var center = V.DenseOfArray(new float[] { (float)((circle.Center.X)), (float)((circle.Center.Y)), 1 });
            var transformedCenter = transformLocal * center;
            var centerX = transformedCenter[0];
            var centerY = transformedCenter[1];

            circle.Center.X = centerX;
            circle.Center.Y = centerY;
            circle.Radius = radiusX;
        }

        public static void UpdateTransform(this EllipseEntity ellipse, Matrix<float> transform)
        {
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var transformLocal = transform;
            var localTransformWithoutTraslation = M.DenseOfArray(transformLocal.ToArray());
            localTransformWithoutTraslation[0, 2] = 0;
            localTransformWithoutTraslation[1, 2] = 0;



            var center = V.DenseOfArray(new float[] { (float)((ellipse.Center.X)), (float)((ellipse.Center.Y)), 1 });
            var transformedCenter = transformLocal * center;
            var centerX = transformedCenter[0];
            var centerY = transformedCenter[1];

            var radius = V.DenseOfArray(new float[] { (float)((ellipse.RadiusX)), (float)((ellipse.RadiusY)), 1 });
            var transformedRadius = localTransformWithoutTraslation * radius;
            var radiusX = transformedRadius[0];
            var radiusY = transformedRadius[1];
            ellipse.Center.X = centerX;
            ellipse.Center.Y = centerY;
            ellipse.RadiusX = radiusX;
            ellipse.RadiusY = radiusY;
        }
    }
}
