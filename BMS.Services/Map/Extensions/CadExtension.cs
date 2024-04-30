using ACadSharp;
using ACadSharp.Tables;
using BMS.Domain.Geometry;
using BMS.Domain.Geometry.Point;
using MongoDB.Driver.Core.Clusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map.Extensions
{
    public static class CadExtension
    {
        public static SizeEntity GetBounds(this CadDocument cad)
        {
            if (cad == null) throw new ArgumentNullException();

            if (cad.VPorts == null || cad.VPorts.Count == 0)
                throw new ArgumentNullException();

            var port = cad.VPorts.ElementAt(0);

            if (port == null) throw new ArgumentNullException();

            var size = new SizeEntity();
            var width = (float)(port.ViewHeight * port.AspectRatio);
            var height = (float)(port.ViewHeight);
            size.Height = height;
            size.Width = width;
            size.X = (float)(port.Center.X - (width / 2));
            size.Y = (float)(port.Center.Y - (height / 2));

            return size;
        }

        public static SizeEntity GetBounds(this BlockRecord cad)
        {
            if (cad == null) throw new ArgumentNullException();

            if (cad.Viewports == null || cad.Viewports.Count == 0)
                throw new ArgumentNullException();
            var port = cad.Viewports.ElementAt(0);

            if (port == null) throw new ArgumentNullException();

            var size = new SizeEntity();
            var width = (float)(port.Width);
            var height = (float)(port.Height);
            size.Height = height;
            size.Width = width;
            size.X = (float)(port.Center.X - (width / 2));
            size.Y = (float)(port.Center.Y - (height / 2));

            return size;
        }
    }
}
