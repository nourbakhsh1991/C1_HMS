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
using ACadSharp.Entities;

namespace BMS.Domain.Converters
{
    public class EllipseConverter : CADConverter
    {
        private ACadSharp.Entities.Ellipse ellipse = null;
        private System.Drawing.PointF _center;
        private float _radiusX;
        private float _radiusY;

        private Domain.Models.Map.Layer _layer;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;


        public EllipseConverter(ACadSharp.Entities.Ellipse ellipse, Matrix<float> transform, Vector<float> mapScale, Domain.Models.Map.Layer layer = null)
        {
            _layer = layer;
            this.ellipse = ellipse;
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

            var end = V.DenseOfArray(new float[] { (float)((ellipse.EndPoint.X)), (float)((ellipse.EndPoint.Y)), 1 });
            var transformedEndPoint = localTransformWithoutTraslation * end;
            var endPointX = transformedEndPoint[0] + transformedCenter[0];
            var endPointY = transformedEndPoint[1] + transformedCenter[1];


            var majorRadius = MathF.Sqrt((centerX - endPointX) * (centerX - endPointX) +
                                            (centerY - endPointY) * (centerY - endPointY));
            var minorRadius = MathF.Abs(majorRadius * (float)ellipse.RadiusRatio);

            _radiusX = endPointX > endPointY ? majorRadius : minorRadius;
            _radiusY = endPointX > endPointY ? minorRadius : majorRadius;

            //var minRatio = MathF.Min(mapScale[0], mapScale[1]);

            _strokeWidth = (int)ellipse.LineWeight;
            _strokeColor =
                        (ellipse.Color.IsByLayer ? -1 :
                        (ellipse.Color.IsByBlock ? -2 : (
                        ellipse.Color.IsTrueColor ? ellipse.Color.TrueColor :
                        ellipse.Color.Index)));

            _center = new System.Drawing.PointF(centerX, centerY);
            //_strokeWidth = _strokeWidth * minRatio;
        }

        public System.Drawing.PointF GetCenter()
        {
            return _center;
        }

        public float GetRadiusX()
        {
            return _radiusX;
        }
        public float GetRadiusY()
        {
            return  _radiusY;
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
