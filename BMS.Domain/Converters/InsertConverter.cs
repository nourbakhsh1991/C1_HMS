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
    public class InsertConverter : CADConverter
    {
        private SvgGroup _group = null;
        private List<SvgPath> _path = new List<SvgPath>();
        private List<float> _lineWidth = new List<float>();
        private List<System.Drawing.Color> _lineColor = new List<System.Drawing.Color>();
        private ACadSharp.Entities.Insert insert = null;
        private System.Drawing.PointF _point;
        private List<CADConverter> _converters = new List<CADConverter>();
        public InsertConverter(ACadSharp.Entities.Insert insert, Matrix<float> transform)
        {
            _group = new SvgGroup();
            this.insert = insert;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            var insertScale = TransformHelper.GetScaleMatrix((float)insert.XScale, (float)insert.YScale);
            var insertTranslate = TransformHelper.GetTranslateMatrix((float)insert.InsertPoint.X, (float)insert.InsertPoint.Y);
            var insertRotation = TransformHelper.GetRotationMatrix((float)insert.Rotation);
            var transformMatrix = insertTranslate * insertScale * insertRotation ;
            var totalMatrix = transform * transformMatrix;
            _group.AddStyle("fill", $"transparent", 0);
            _group.CustomAttributes.Add("Layer", insert.Layer.Name);
            if(insert.Layer.Name == "Exhaust")
            {
                var asdfs = 0;
            }
            var color = getStrokeColor(insert);
            var width = getStrokeWidth(insert) / 100;
            _group.AddStyle("stroke-width", width.ToString(), 0);
            _group.AddStyle("stroke", ColorIndex.GetColorHex(color) , 0);
            var segments = new SvgPathSegmentList();
            var blockLines = insert.Block.Entities.Where(a => a.ObjectType == ObjectType.LINE).Select(a => (ACadSharp.Entities.Line)a).ToList();
            foreach (var line in blockLines)
            {
            //    var svgLine = new LineConverter(line, totalMatrix);
            //    _converters.Add(svgLine);
            //    var lineColor = svgLine.GetStrokeColor();
            //    var lineWidth = svgLine.GetStrokeWidth();
            //    var found = false;
            //    for(int i = 0;i< _path.Count; i++)
            //    {
            //        if (_lineWidth[i] == lineWidth && _lineColor[i] == lineColor)
            //        {
            //            found = true;
            //            foreach (var seg in svgLine.GetSegments())
            //                _path[i].PathData.Add(seg);
            //            break;
            //        }
            //    }
            //    if (!found)
            //    {
            //        var newPath = new SvgPath();
            //        newPath.PathData = new SvgPathSegmentList();
            //        foreach (var seg in svgLine.GetSegments())
            //            newPath.PathData.Add(seg);
            //        newPath.AddStyle("stroke", ColorIndex.GetColorHex(lineColor), 0);
            //        newPath.AddStyle("stroke-width", lineWidth.ToString(), 0);
            //        newPath.AddStyle("fill", $"transparent", 0);
            //        _group.Children.Add(newPath);
            //        _path.Add(newPath);
            //        _lineColor.Add(lineColor);
            //        _lineWidth.Add(lineWidth);
            //    }
            //}
            //var blockArcs = insert.Block.Entities.Where(a => a.ObjectType == ObjectType.ARC).Select(a => (ACadSharp.Entities.Arc)a).ToList();
            //foreach (var arc in blockArcs)
            //{
            //    var svgArc = new ArcConverter(arc, totalMatrix , (float)insert.Rotation);
            //    _converters.Add(svgArc);
            //    var arcColor = svgArc.GetStrokeColor();
            //    var arcWidth = svgArc.GetStrokeWidth();
            //    var found = false;
            //    for (int i = 0; i < _path.Count; i++)
            //    {
            //        if (_lineWidth[i] == arcWidth && _lineColor[i] == arcColor)
            //        {
            //            found = true;
            //            foreach (var seg in svgArc.GetSegments())
            //                _path[i].PathData.Add(seg);
            //            break;
            //        }
            //    }
            //    if (!found)
            //    {
            //        var newPath = new SvgPath();
            //        newPath.PathData = new SvgPathSegmentList();
            //        foreach (var seg in svgArc.GetSegments())
            //            newPath.PathData.Add(seg);
            //        newPath.AddStyle("stroke", ColorIndex.GetColorHex(arcColor), 0);
            //        newPath.AddStyle("stroke-width", arcWidth.ToString(), 0);
            //        newPath.AddStyle("fill", $"transparent", 0);
            //        _group.Children.Add(newPath);
            //        _path.Add(newPath);
            //        _lineColor.Add(arcColor);
            //        _lineWidth.Add(arcWidth);
            //    }
            //}
            //var blockMText = insert.Block.Entities.Where(a => a.ObjectType == ObjectType.MTEXT).Select(a => (ACadSharp.Entities.MText)a).ToList();
            //foreach (var mText in blockMText)
            //{
            //    var labelSvg = new SvgText(mText.Value);
            //    var insertPoint = V.DenseOfArray(new float[] { (float)mText.InsertPoint.X, (float)mText.InsertPoint.Y, 1 });
            //    var transformedStart = totalMatrix * insertPoint;
            //    var labelColor = getStrokeColor(mText);
                
            //    labelSvg.X = new SvgUnitCollection() { transformedStart[0] };
            //    labelSvg.Y = new SvgUnitCollection() { transformedStart[1] };
            //    labelSvg.Color = new SvgColourServer(labelColor);
            //    _group.Children.Add(labelSvg);
            }
        }

        public SvgGroup Get()
        {
            return _group;
        }

        public List<CADConverter> GetConverters()
        {
            return _converters;
        }


        private int getStrokeWidth(ACadSharp.Entities.Entity entity)
        {
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;
            else if (entity.LineWeight == LineweightType.ByLayer)
                return getStrokeWidth(entity.Layer);
            else if (entity.LineWeight == LineweightType.ByBlock)
                return getStrokeWidth(this.insert);
            else if (entity.LineWeight == LineweightType.Default)
                return (int)LineweightType.W25;

            return (int)LineweightType.W25;
        }

        private int getStrokeWidth(ACadSharp.Tables.Layer entity)
        {
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;


            return (int)LineweightType.W25;
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
