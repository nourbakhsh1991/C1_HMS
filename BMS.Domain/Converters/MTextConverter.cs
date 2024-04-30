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
    public class MTextConverter : CADConverter
    {
        private ACadSharp.Entities.MText text = null;

        private Domain.Models.Map.Layer _layer;
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private System.Drawing.PointF _insertPoint;
        private System.Drawing.PointF _size;
        private string _text;
        public MTextConverter(ACadSharp.Entities.MText text, Matrix<float> transform, Domain.Models.Map.Layer layer = null)
        {
            this.text = text;
            _text = text.Value;
            _layer = layer;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            var widthRect = text.RectangleWitdth;
            var heightRect = text.Height;

            var textSize = V.DenseOfArray(new float[] { (float)widthRect, (float)heightRect, 1 });
            
            var globalScaleWithoutTraslation = M.DenseOfArray(transform.ToArray());
            globalScaleWithoutTraslation[0, 2] = 0;
            globalScaleWithoutTraslation[1, 2] = 0;

            var insertPoint = V.DenseOfArray(new float[] { (float)text.InsertPoint.X, (float)text.InsertPoint.Y, 1 });
            var transformedStart = transform * insertPoint;
            var transformedSize = globalScaleWithoutTraslation * textSize;

            _insertPoint = new System.Drawing.PointF(transformedStart[0], transformedStart[1]);
            _size = new System.Drawing.PointF(transformedSize[0], transformedSize[1]);

            _strokeWidth = (int)text.LineWeight;
            _strokeColor =
                        (text.Color.IsByLayer ? -1 :
                        (text.Color.IsByBlock ? -2 : (
                        text.Color.IsTrueColor ? text.Color.TrueColor :
                        text.Color.Index)));

        }


        public string Get()
        {
            return _text;
        }

        public System.Drawing.PointF GetInsertPoint()
        {
            return _insertPoint;
        }

        public System.Drawing.PointF GetSize()
        {
            return _size;
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
