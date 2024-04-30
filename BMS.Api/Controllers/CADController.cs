//using Aspose.CAD;
//using Aspose.Imaging;
//using Aspose.CAD.ImageOptions.SvgOptionsParameters;
using Microsoft.AspNetCore.Mvc;
using ACadSharp.IO;
using ACadSharp;
using MediatR;
using System.IO;
using Svg;
using static System.Net.WebRequestMethods;
using BMS.Shared.Helpers;
using BMS.Domain.Converters;
using Svg.Pathing;
using netDxf.Collections;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;
using netDxf;
using System.Diagnostics;
using netDxf.Objects;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Http.Features;
using ACadSharp.Entities;
using System.Security.Cryptography.Xml;
using System.Collections.Generic;
using BMS.Domain.Geometry.Line;
using BMS.Domain.Geometry;
using BMS.Domain.Geometry.Arc;
using BMS.Domain.Geometry.Insert;
using System.Xml;
using BMS.Domain.Geometry.Circle;
using BMS.Domain.Geometry.Text;
using BMS.Domain.Geometry.Point;

namespace BMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CADController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var maxSize = 2000;
            // Load the input DWG file
            //Aspose.CAD.Image image = Aspose.CAD.Image.Load("Files/Raw.dwg");


            // Initialize SvgOptions class object
            //Aspose.CAD.ImageOptions.SvgOptions options = new Aspose.CAD.ImageOptions.SvgOptions();

            //var width = image.Width;
            //var height = image.Height;
            //if (width > height)
            //{
            //    int newHeight = (int)((height * maxSize) / (width * 1.0f));
            //    width = maxSize;
            //    height = newHeight;
            //}
            //else
            //{
            //    int newWidth = (int)((width * maxSize) / (height * 1.0f));
            //    height = maxSize;
            //    width = newWidth;
            //}

            //Aspose.CAD.ImageOptions.CadRasterizationOptions cadRasterizationOptions = new Aspose.CAD.ImageOptions.CadRasterizationOptions();
            //options.VectorRasterizationOptions = cadRasterizationOptions;
            //cadRasterizationOptions.PageSize = new Aspose.CAD.SizeF(width, height);
            //// Set SVG color mode
            //options.ColorType = Aspose.CAD.ImageOptions.SvgOptionsParameters.SvgColorMode.Grayscale;
            //options.TextAsShapes = true;
            //// Save output SVG file
            //image.Save(Directory.GetCurrentDirectory() + "/sample.svg", options);

            CadDocument doc = DwgReader.Read("Files/Raw.dwg");



            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            var enetitiesList = doc.Entities.Select(a => a.ObjectName).Distinct().ToList();
            foreach (var entity in doc.Entities)
            {
                if (entity is ACadSharp.Entities.Line line)
                {
                    if (line.StartPoint.X < minX) minX = (float)line.StartPoint.X;
                    if (line.StartPoint.X > maxX) maxX = (float)line.StartPoint.X;
                    if (line.StartPoint.Y < minY) minY = (float)line.StartPoint.Y;
                    if (line.StartPoint.Y > maxY) maxY = (float)line.StartPoint.Y;
                    if (line.EndPoint.X < minX) minX = (float)line.EndPoint.X;
                    if (line.EndPoint.X > maxX) maxX = (float)line.EndPoint.X;
                    if (line.EndPoint.Y < minY) minY = (float)line.EndPoint.Y;
                    if (line.EndPoint.Y > maxY) maxY = (float)line.EndPoint.Y;

                }
                else if(entity is ACadSharp.Entities.LwPolyline poly)
                {
                    foreach(var point in poly.Vertices)
                    {
                        if (point.Location.X < minX) minX = (float)point.Location.X;
                        if (point.Location.X > maxX) maxX = (float)point.Location.X;
                        if (point.Location.Y < minY) minY = (float)point.Location.Y;
                        if (point.Location.Y > maxY) maxY = (float)point.Location.Y;
                    }
                }
            }
            var width = maxX - minX;
            var height = maxY - minY;
            var ratio = width / height;
            maxSize = 500;
           
            var shapeWidth = (width > height) ? maxSize : maxSize * ratio;
            var shapeHeight = (height > width) ? maxSize : maxSize / ratio;

            var transformZero = TransformHelper.GetTranslateMatrix(-minX, -minY);
            var flipCad = TransformHelper.GetScaleMatrix(1, -1);
            var transformCad = TransformHelper.GetTranslateMatrix(0, shapeHeight);
            //var transformCad = TransformHelper.GetTranslateMatrix(0, maxSize);
            var scaleCad = TransformHelper.GetScaleMatrix(shapeWidth / width, shapeHeight / height);
            //var scaleCad = TransformHelper.GetScaleMatrix(.01f, .01f);
            var globalScale = /*scaleCad * transformCad * flipCad **/ transformCad * scaleCad * flipCad * transformZero;
            var globalSize = V.DenseOfArray(new float[] { shapeWidth, shapeHeight });

            var lines = doc.Entities.Where(a => a.ObjectType == ObjectType.LINE).Select(a => (ACadSharp.Entities.Line)a).ToList();

            var svgDoc = new SvgDocument()
            {
                Width = maxSize,
                Height = maxSize,
                ViewBox = new SvgViewBox(0, 0, maxSize, maxSize),
            };
            var group = new SvgGroup();
            var res = new List<object>();
            svgDoc.Children.Add(group);
            foreach (var line in lines)
            {

                //var svgLine = new LineConverter(line, globalScale);
                //group.Children.Add((svgLine.Get()));
                //    res.Add(new LineEntity
                //    {
                //        CapType = "",
                //        DashArray = svgLine.Get().StrokeDashArray.Select(a => a.Value).ToList(),
                //        Start = new Domain.Geometry.Point.PointEntity
                //        {
                //            X = svgLine.Get().StartX.Value,
                //            Y = svgLine.Get().StartY.Value,
                //        },
                //        End = new Domain.Geometry.Point.PointEntity
                //        {
                //            X = svgLine.Get().EndX.Value,
                //            Y = svgLine.Get().EndY.Value,
                //        },
                //        StrokeColor = ColorIndex.GetColorHex(svgLine.GetStrokeColor()),
                //        StrokeWidth = svgLine.GetStrokeWidth(),
                //        Type = "LineEntity"
                //    });
            }

            var blockArcs = doc.Entities.Where(a => a.ObjectType == ObjectType.ARC).Select(a => (ACadSharp.Entities.Arc)a).ToList();
            foreach (var arc in blockArcs)
            {
                //var svgArc = new ArcConverter(arc, globalScale);
                //var arcColor = svgArc.GetStrokeColor();
                //var arcWidth = svgArc.GetStrokeWidth();
                //res.Add(new ArcEntity
                //{
                //    CapType = "",
                //    Center = new Domain.Geometry.Point.PointEntity
                //    {
                //        X = svgArc.GetCenter().X,
                //        Y = svgArc.GetCenter().Y,
                //    },
                //    StrokeColor = ColorIndex.GetColorHex(svgArc.GetStrokeColor()),
                //    StrokeWidth = svgArc.GetStrokeWidth(),
                //    EndAngle = svgArc.GetEndAngle(),
                //    StartAngle = svgArc.GetStartAngle(),
                //    Radius = svgArc.GetRadius(),
                //    Type = "ArcEntity"

                //});
            }

            var blockSpline = doc.Entities.Where(a => a.ObjectType == ObjectType.SPLINE).Select(a => (ACadSharp.Entities.Spline)a).ToList();
            foreach (var spline in blockSpline)
            {
                //var svgArc = new ArcConverter(arc, globalScale);
                //var arcColor = svgArc.GetStrokeColor();
                //var arcWidth = svgArc.GetStrokeWidth();
                //res.Add(new ArcEntity
                //{
                //    CapType = "",
                //    Center = new Domain.Geometry.Point.PointEntity
                //    {
                //        X = svgArc.GetCenter().X,
                //        Y = svgArc.GetCenter().Y,
                //    },
                //    StrokeColor = ColorIndex.GetColorHex(svgArc.GetStrokeColor()),
                //    StrokeWidth = svgArc.GetStrokeWidth(),
                //    EndAngle = svgArc.GetEndAngle(),
                //    StartAngle = svgArc.GetStartAngle(),
                //    Type = "ArcEntity"

                //});
            }

            var blockCircles = doc.Entities.Where(a => a.ObjectType == ObjectType.CIRCLE).Select(a => (ACadSharp.Entities.Circle)a).ToList();
            foreach (var circle in blockCircles)
            {

                //var scgCircle = new CircleConverter(circle, globalScale);

                //var circleColor = scgCircle.GetStrokeColor();
                //var circleWidth = scgCircle.GetStrokeWidth();
                //res.Add(new CircleEntity
                //{
                //    CapType = "",
                //    Center = new Domain.Geometry.Point.PointEntity
                //    {
                //        X = scgCircle.GetCenter().X,
                //        Y = scgCircle.GetCenter().Y,
                //    },
                //    StrokeColor = ColorIndex.GetColorHex(circleColor),
                //    StrokeWidth = circleWidth,
                //    Type = "CircleEntity"

                //});
            }

            var blockEllipses = doc.Entities.Where(a => a.ObjectType == ObjectType.ELLIPSE).Select(a => (ACadSharp.Entities.Ellipse)a).ToList();
            foreach (var ellipse in blockEllipses)
            {

                //var scgEllipse = new EllipseConverter(ellipse, globalScale);
                //var ellipseColor = scgEllipse.GetStrokeColor();
                //var ellipseWidth = scgEllipse.GetStrokeWidth();
                //res.Add(new EllipseEntity
                //{
                //    CapType = "",
                //    Center = new Domain.Geometry.Point.PointEntity
                //    {
                //        X = scgEllipse.GetCenter().X,
                //        Y = scgEllipse.GetCenter().Y,
                //    },
                //    StrokeColor = ColorIndex.GetColorHex(ellipseColor),
                //    StrokeWidth = ellipseWidth,
                //    RadiusX = scgEllipse.GetRadiusMajor(),
                //    RadiusY = scgEllipse.GetRadiusMinor(),
                //    Type = "EllipseEntity"

                //});
            }

            //var blockMText = doc.Entities.Where(a => a.ObjectType == ObjectType.MTEXT).Select(a => (ACadSharp.Entities.MText)a).ToList();
            //foreach (var mtext in blockMText)
            //{

            //    var svgText = new MTextConverter(mtext, globalScale);
            //    //var svgArc = new ArcConverter(arc, globalScale);
            //    var textInsertPoint = svgText.GetInsertPoint();
            //    var textSize = svgText.GetSize();
            //    var textColor = svgText.GetStrokeColor();
            //    var textWidth = svgText.GetStrokeWidth();
            //    res.Add(new MTextEntity
            //    {
            //        CapType = "",
            //        StrokeColor = ColorIndex.GetColorHex(textColor),
            //        StrokeWidth = textWidth,
            //        InsertPoint = new Domain.Geometry.Point.PointEntity
            //        {
            //            X = textInsertPoint.X,
            //            Y = textInsertPoint.Y,
            //        },
            //        Size = new Domain.Geometry.Point.PointEntity
            //        {
            //            X = textSize.X,
            //            Y = textSize.Y,
            //        },
            //        Text = svgText.Get().Text,
            //        Type = "MTextEntity"

            //    });
            //}
            var layers = new List<string>();
            var blockLWPolyLine = doc.Entities.Where(a => a.ObjectType == ObjectType.LWPOLYLINE).Select(a => (ACadSharp.Entities.LwPolyline)a).ToList();
            foreach (var lwpolyline in blockLWPolyLine)
            {
                //layers.Add(lwpolyline.Layer.Name);
                //var svgPolyLine = new LwPolyLineConverter(lwpolyline, globalScale);
                
                //    var lwpolyColor = ColorIndex.GetColorHex(svgPolyLine.GetStrokeColor());
                //    var lwpilyWidth = svgPolyLine.GetStrokeWidth();
                //    var lwpoly = new LwPolyLineEntity
                //    {
                //        CapType = "",
                //        StrokeColor = lwpolyColor,
                //        StrokeWidth = lwpilyWidth,
                //        DashArray = svgPolyLine.GetDashArray(),
                //        IsClosed = lwpolyline.IsClosed,
                //        Vertices = new List<PointEntity>(),
                //        Type = "LwPolyLineEntity"
                //    };
                //    foreach (var svgLine in svgPolyLine.Get())
                //    {
                //        lwpoly.Vertices.Add(new PointEntity
                //        {
                //            CapType = "",
                //            X = svgLine.X,
                //            Y = svgLine.Y,
                //            Type = "PointEntity"
                //        });
                //    }
                //    res.Add(lwpoly);
                
            }
            var inserts = doc.Entities.Where(a => a.ObjectType == ObjectType.INSERT).Select(a => (ACadSharp.Entities.Insert)a).ToList();
            foreach (var insert in inserts)
            {
                var svgInsert = new InsertConverter(insert, globalScale);
                group.Children.Add((svgInsert.Get()));
                var insertEntity = new InsertEntity
                {
                    CapType = "",
                    Entities = new List<Domain.Models.Map.Entity>(),
                    Type = "InsertEntity"

                };
                foreach (var converter in svgInsert.GetConverters())
                {
                    //if (converter is LineConverter lineConverter)
                    //{
                    //    insertEntity.Entities.Add(new LineEntity
                    //    {
                    //        CapType = "",
                    //        DashArray = lineConverter.Get().StrokeDashArray.Select(a => a.Value).ToList(),
                    //        Start = new Domain.Geometry.Point.PointEntity
                    //        {
                    //            X = lineConverter.Get().StartX.Value,
                    //            Y = lineConverter.Get().StartY.Value,
                    //        },
                    //        End = new Domain.Geometry.Point.PointEntity
                    //        {
                    //            X = lineConverter.Get().EndX.Value,
                    //            Y = lineConverter.Get().EndY.Value,
                    //        },
                    //        StrokeColor = ColorIndex.GetColorHex(lineConverter.GetStrokeColor()),
                    //        StrokeWidth = lineConverter.GetStrokeWidth(),
                    //        Type = "LineEntity"
                    //    });
                    //}
                    //if (converter is ArcConverter arcConverter)
                    //{
                    //    insertEntity.Entities.Add(new ArcEntity
                    //    {
                    //        CapType = "",
                    //        Center = new Domain.Geometry.Point.PointEntity
                    //        {
                    //            X = arcConverter.GetCenter().X,
                    //            Y = arcConverter.GetCenter().Y,
                    //        },
                    //        StrokeColor = ColorIndex.GetColorHex(arcConverter.GetStrokeColor()),
                    //        StrokeWidth = arcConverter.GetStrokeWidth(),
                    //        EndAngle = arcConverter.GetEndAngle(),
                    //        StartAngle = arcConverter.GetStartAngle(),
                    //        Radius = arcConverter.GetRadius(),
                    //        Type = "ArcEntity"

                    //    });
                    //}
                }
                res.Add(insertEntity);
            }
            layers = layers.Distinct().ToList();
            return Ok(res);

            //var inserts = doc.Entities.Where(a => a.ObjectType == ObjectType.INSERT).Select(a => (ACadSharp.Entities.Insert)a).ToList();
            //foreach (var insert in inserts)
            //{
            //    var svgInsert = new InsertConverter(insert, globalScale);
            //    group.Children.Add((svgInsert.Get()));
            //}

            //var blockMText = doc.Entities.Where(a => a.ObjectType == ObjectType.MTEXT).Select(a => (ACadSharp.Entities.MText)a).ToList();
            //foreach (var mText in blockMText)
            //{
            //    var labelSvg = new SvgText(mText.Value);
            //    if (mText.Value == "111")
            //    {
            //        var sdf = 0;
            //    }
            //    var rect = new SvgRectangle();
            //    var widthRect = mText.RectangleWitdth;
            //    var heightRect = mText.Height;
            //    var textSize = V.DenseOfArray(new float[] { (float)widthRect, (float)heightRect, 1 });
            //    var globalScaleWithoutTraslation = M.DenseOfArray(globalScale.ToArray());
            //    globalScaleWithoutTraslation[0, 2] = 0;
            //    globalScaleWithoutTraslation[1, 2] = 0;
            //    var insertPoint = V.DenseOfArray(new float[] { (float)mText.InsertPoint.X, (float)mText.InsertPoint.Y, 1 });
            //    var transformedStart = globalScale * insertPoint;
            //    var transformedSize = scaleCad * textSize;
            //    //labelSvg.CustomAttributes.Add("textLength", transformedSize[0].ToString());
            //    var labelColor = System.Drawing.Color.Black;// //getStrokeColor(mText);
            //    rect.X = transformedStart[0];
            //    rect.Y = transformedStart[1];
            //    rect.Width = transformedSize[0];
            //    rect.Height = transformedSize[1];
            //    rect.Fill = new SvgColourServer(System.Drawing.Color.Transparent);
            //    labelSvg.X = new SvgUnitCollection() { transformedStart[0] };
            //    labelSvg.Y = new SvgUnitCollection() { transformedStart[1] };
            //    labelSvg.LengthAdjust = SvgTextLengthAdjust.SpacingAndGlyphs;
            //    labelSvg.TextLength = new SvgUnit(transformedSize[1]);
            //    labelSvg.FontSize = new SvgUnit(SvgUnitType.Em, 0.1f);

            //    // rect.Children.Add(labelSvg);
            //    labelSvg.Color = new SvgColourServer(labelColor);
            //    group.Children.Add(labelSvg);
            //}

            //svgDoc.Write(Directory.GetCurrentDirectory() + "/sample.svg");
            //var str = svgDoc.GetXML();
            //var start = str.IndexOf("<svg");
            //str = str.Substring(start);
            //str.Replace("NaN", "0");
            //return Ok(str);
        }

    }
}
