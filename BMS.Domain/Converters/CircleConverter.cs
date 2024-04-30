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

namespace BMS.Domain.Converters
{
    public class CircleConverter : CADConverter
    {
        private ACadSharp.Entities.Circle circle = null;
        private System.Drawing.PointF _center;
        private float _radius;
        private Domain.Models.Map.Layer _layer;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;

        public CircleConverter(ACadSharp.Entities.Circle circle, Matrix<float> transform, Vector<float> mapScale, Domain.Models.Map.Layer layer = null)
        {
            _layer = layer;
            this.circle = circle;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            var transformLocal = transform;
            var radius = V.DenseOfArray(new float[] { (float)((circle.Radius)), (float)((circle.Radius)), 1 });
            var localTransformWithoutTraslation = M.DenseOfArray(transformLocal.ToArray());
            localTransformWithoutTraslation[0, 2] = 0;
            localTransformWithoutTraslation[1, 2] = 0;

            var transformedRadius = localTransformWithoutTraslation * radius;
            var radiusX = MathF.Abs( transformedRadius[0]);

            var center = V.DenseOfArray(new float[] { (float)((circle.Center.X)), (float)((circle.Center.Y)), 1 });
            var transformedCenter = transformLocal * center;
            var centerX = transformedCenter[0];
            var centerY = transformedCenter[1];

            //var minRatio = MathF.Min(mapScale[0], mapScale[1]);
            _strokeWidth = (int)circle.LineWeight;
            _strokeColor =
                        (circle.Color.IsByLayer ? -1 :
                        (circle.Color.IsByBlock ? -2 : (
                        circle.Color.IsTrueColor ? circle.Color.TrueColor :
                        circle.Color.Index))); 
            _center = new System.Drawing.PointF(centerX, centerY);
            _radius = radiusX;

            //_strokeWidth = _strokeWidth * minRatio;
        }

        public System.Drawing.PointF GetCenter()
        {
            return _center;
        }

        public float GetRadius()
        {
            return _radius;
        }

        public float GetArea()
        {
            return MathF.PI * _radius * 2;
        }


        public int GetStrokeWidth()
        {
            return _strokeWidth;
        }

        public int GetStrokeColor()
        {
            return _strokeColor;
        }

        private int getStrokeWidth(ACadSharp.Entities.Entity entity)
        {
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;
            else if (entity.LineWeight == LineweightType.ByLayer && _layer != null)
                return (int)_layer.StrokeWidth;
            else if (entity.LineWeight == LineweightType.ByLayer)
                return getStrokeWidth(entity.Layer);
            else if (entity.LineWeight == LineweightType.Default)
                return (int)LineweightType.W5;
            return (int)LineweightType.W5;
        }

        private int getStrokeWidth(ACadSharp.Tables.Layer entity)
        {
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;
            return (int)LineweightType.W5;
        }

        private System.Drawing.Color getStrokeColor(ACadSharp.Entities.Entity entity)
        {
            //if (entity.Color.IsByLayer && _layer != null)
            //    return System.Drawing.Color.FromName(_layer.Color);
            //else if (entity.Color.IsByLayer)
            //    return ColorIndex.GetColorByIndex(entity.Layer.Color.Index);
            //else if (entity.Color.TrueColor == -1)
            //    return ColorIndex.GetColorByIndex(entity.Color.Index);
            //else if (entity.Color.IsByBlock)
            //    return ColorIndex.GetColorByIndex(0);
            return ColorIndex.GetColorByIndex(entity.Color.Index);
        }
    }
}
