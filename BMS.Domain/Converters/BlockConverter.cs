using ACadSharp;
using BMS.Domain.Geometry.Block;
using BMS.Domain.Geometry.Insert;
using BMS.Shared.Helpers;
using ExCSS;
using MathNet.Numerics.LinearAlgebra;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Converters
{
    public class BlockConverter : CADConverter
    {
        private BlockEntity _group = new BlockEntity() ;
        private List<SvgPath> _path = new List<SvgPath>();
        private List<float> _lineWidth = new List<float>();
        private List<System.Drawing.Color> _lineColor = new List<System.Drawing.Color>();
        private ACadSharp.Tables.BlockRecord _block = null;
        private System.Drawing.PointF _point;
        private List<CADConverter> _converters = new List<CADConverter>();
        public BlockConverter(ACadSharp.Tables.BlockRecord block, Matrix<float> transform, Vector<float> mapScale)
        {
            _block = block;
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;
            _group.Type = "BlockEntity";
            _group.Preview = block.Preview;
            //_group.Transform = transform.to;
            _group.StrokeColor = -2; //ColorIndex.GetColorHex(ColorIndex.GetColorByIndex(0)) ;
            var minRatio = MathF.Min(mapScale[0], mapScale[1]);
            _group.StrokeWidth = -2; //getStrokeWidth(block) * minRatio;
            _group.Fill = -2;
        }

        public BlockEntity Get()
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
                return getStrokeWidth(_block);
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

        private int getStrokeWidth(ACadSharp.Tables.BlockRecord entity)
        {

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

        private System.Drawing.Color getStrokeColor(ACadSharp.Tables.BlockRecord entity)
        {
            return ColorIndex.GetColorByIndex(0);
        }


    }
}
