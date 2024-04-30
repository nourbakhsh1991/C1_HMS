using ACadSharp;
using BMS.Shared.Helpers;
using ExCSS;
using MathNet.Numerics.LinearAlgebra;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Converters
{
    public class InsertConverter2 : CADConverter
    {
        private List<float> _lineWidth = new List<float>(); 
        private int _strokeColor = -1;
        private int _strokeWidth = 5;
        private Domain.Models.Map.Layer _layer;
        private ACadSharp.Entities.Insert insert = null;
        private System.Drawing.PointF _point;
        private List<CADConverter> _converters = new List<CADConverter>();
        public InsertConverter2(ACadSharp.Entities.Insert insert, Matrix<float> transform, Vector<float> mapScale, Domain.Models.Map.Layer layer = null)
        {
            _layer = layer;
            this.insert = insert;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var insertScale = TransformHelper.GetScaleMatrix((float)insert.XScale, (float)insert.YScale);
            var insertTranslate = TransformHelper.GetTranslateMatrix((float)insert.InsertPoint.X, (float)insert.InsertPoint.Y);
            var insertRotation = TransformHelper.GetRotationMatrix((float)insert.Rotation);
            var transformMatrix = insertTranslate * insertScale * insertRotation ;
            var totalMatrix = transform * transformMatrix;

            _strokeWidth = (int)insert.LineWeight;
            _strokeColor =
                        (insert.Color.IsByLayer ? -1 :
                        (insert.Color.IsByBlock ? -2 : (
                        insert.Color.IsTrueColor ? insert.Color.TrueColor :
                        insert.Color.Index)));
        }

        public ACadSharp.Entities.Insert Get()
        {
            return this.insert;
        }

        public List<CADConverter> GetConverters()
        {
            return _converters;
        }
        public int GetStrokeWidth()
        {
            return _strokeWidth;
        }

        public int GetStrokeColor()
        {
            return _strokeColor;
        }


    }
}
