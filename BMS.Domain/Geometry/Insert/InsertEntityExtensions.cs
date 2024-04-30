using BMS.Domain.Geometry.Arc;
using BMS.Domain.Geometry.Circle;
using BMS.Domain.Geometry.Line;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Geometry.Insert
{
    public static class InsertEntityExtensions
    {
        public static void UpdateTransform(this InsertEntity insert, Matrix<float> transform)
        {
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            foreach (var entity in insert.Entities)
            {
                if (entity.Data is LineEntity line)
                {
                    line.UpdateTransform(transform);
                }
                else if (entity.Data is ArcEntity arc)
                {
                    arc.UpdateTransform(transform);
                }
                else if (entity.Data is CircleEntity circle)
                {
                    circle.UpdateTransform(transform);
                }
                else if (entity.Data is EllipseEntity ellipse)
                {
                    ellipse.UpdateTransform(transform);
                }
                else if (entity.Data is LwPolyLineEntity lwpolyline)
                {
                    lwpolyline.UpdateTransform(transform);
                }
                else if (entity.Data is InsertEntity insert2)
                {
                    insert2.UpdateTransform(transform);
                }
            }
        }
    }
}
