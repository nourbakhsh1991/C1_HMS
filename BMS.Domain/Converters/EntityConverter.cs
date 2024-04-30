using BMS.Domain.Geometry;
using BMS.Domain.Geometry.Arc;
using BMS.Domain.Geometry.Circle;
using BMS.Domain.Geometry.Insert;
using BMS.Domain.Geometry.Line;
using BMS.Domain.Geometry.Point;
using BMS.Domain.Geometry.Text;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Converters
{
    public class EntityConverter : CADConverter
    {
        public List<GeometryEntity> GeometryEntities { get; set; } = new List<GeometryEntity> { };
        public List<Models.Map.Entity> Entities { get; set; } = new List<Models.Map.Entity> { };
        public EntityConverter(List<ACadSharp.Entities.Entity> entities, Domain.Models.Map.Layer layer, Matrix<float> transform, Vector<float> mapScale, float rotation = 0)
        {
            foreach (var entity in entities)
            {

                if (entity is ACadSharp.Entities.Line line)
                {
                    var svgLine = new LineConverter(line, transform, layer);
                    var lineEntity = new LineEntity
                    {
                        CapType = "Round",
                        DashArray = svgLine.GetSegments(),
                        Start = new PointEntity
                        {
                            X = svgLine.StartX,
                            Y = svgLine.StartY,
                        },
                        End = new PointEntity
                        {
                            X = svgLine.EndX,
                            Y = svgLine.EndY,
                        },
                        StrokeColor = svgLine.GetStrokeColor(),
                        StrokeWidth = svgLine.GetStrokeWidth(),
                        Type = "LineEntity",
                        LayerId = layer.Id,
                    };
                    GeometryEntities.Add(lineEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "LineEntity",
                        LayerId = layer.Id,
                        Data = lineEntity,

                    });
                }
                else if (entity is ACadSharp.Entities.Arc arc)
                {
                    var svgArc = new ArcConverter(arc, transform, mapScale, rotation, layer);
                    var arcColor = svgArc.GetStrokeColor();
                    var arcWidth = svgArc.GetStrokeWidth();
                    var arcEntity = new ArcEntity
                    {
                        CapType = "Round",
                        Center = new Domain.Geometry.Point.PointEntity
                        {
                            X = svgArc.GetCenter().X,
                            Y = svgArc.GetCenter().Y,
                        },
                        StrokeColor = svgArc.GetStrokeColor(),
                        StrokeWidth = svgArc.GetStrokeWidth(),
                        EndAngle = svgArc.GetEndAngle(),
                        StartAngle = svgArc.GetStartAngle(),
                        Radius = svgArc.GetRadius(),
                        Type = "ArcEntity",
                        LayerId = layer.Id

                    };
                    GeometryEntities.Add(arcEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "ArcEntity",
                        LayerId = layer.Id,
                        Data = arcEntity
                    });
                }
                else if (entity is ACadSharp.Entities.Circle circle)
                {
                    var scgCircle = new CircleConverter(circle, transform, mapScale, layer);

                    var circleColor = scgCircle.GetStrokeColor();
                    var circleWidth = scgCircle.GetStrokeWidth();
                    var circleEntity = new CircleEntity
                    {
                        CapType = "Round",
                        Center = new Domain.Geometry.Point.PointEntity
                        {
                            X = scgCircle.GetCenter().X,
                            Y = scgCircle.GetCenter().Y,
                        },
                        StrokeColor = circleColor,
                        StrokeWidth = circleWidth,
                        Type = "CircleEntity",
                        LayerId = layer.Id,
                        Radius = scgCircle.GetRadius()

                    };
                    GeometryEntities.Add(circleEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "CircleEntity",
                        LayerId = layer.Id,
                        Data = circleEntity
                    });
                }
                else if (entity is ACadSharp.Entities.Ellipse ellipse)
                {
                    var scgEllipse = new EllipseConverter(ellipse, transform, mapScale, layer);
                    var ellipseColor = scgEllipse.GetStrokeColor();
                    var ellipseWidth = scgEllipse.GetStrokeWidth();
                    var ellipseEntity = new EllipseEntity
                    {
                        CapType = "",
                        Center = new Domain.Geometry.Point.PointEntity
                        {
                            X = scgEllipse.GetCenter().X,
                            Y = scgEllipse.GetCenter().Y,
                        },
                        StrokeColor = ellipseColor,
                        StrokeWidth = ellipseWidth,
                        RadiusX = scgEllipse.GetRadiusX(),
                        RadiusY = scgEllipse.GetRadiusY(),
                        Type = "EllipseEntity",
                        LayerId = layer.Id

                    };
                    GeometryEntities.Add(ellipseEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "EllipseEntity",
                        LayerId = layer.Id,
                        Data = ellipseEntity
                    });
                }
                else if (entity is ACadSharp.Entities.MText mtext)
                {
                    var svgText = new MTextConverter(mtext, transform, layer);
                    var textInsertPoint = svgText.GetInsertPoint();
                    var textSize = svgText.GetSize();
                    var textColor = svgText.GetStrokeColor();
                    var textWidth = svgText.GetStrokeWidth();
                    var mtextEntity = new MTextEntity
                    {
                        CapType = "Round",
                        StrokeColor = textColor,
                        StrokeWidth = textWidth,
                        InsertPoint = new Domain.Geometry.Point.PointEntity
                        {
                            X = textInsertPoint.X,
                            Y = textInsertPoint.Y,
                        },
                        Size = new Domain.Geometry.Point.PointEntity
                        {
                            X = textSize.X,
                            Y = textSize.Y,
                        },
                        Text = svgText.Get(),
                        Type = "MTextEntity",
                        LayerId = layer.Id

                    };
                    GeometryEntities.Add(mtextEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "MTextEntity",
                        LayerId = layer.Id,
                        Data = mtextEntity
                    });
                }
                else if (entity is ACadSharp.Entities.LwPolyline lwpolyline)
                {
                    var svgPolyLine = new LwPolyLineConverter(lwpolyline, transform, mapScale, layer);
                    var lwpolyColor = svgPolyLine.GetStrokeColor();
                    var lwpilyWidth = svgPolyLine.GetStrokeWidth();
                    var lwpoly = new LwPolyLineEntity
                    {
                        CapType = "Round",
                        StrokeColor = lwpolyColor,
                        StrokeWidth = lwpilyWidth,
                        DashArray = svgPolyLine.GetDashArray(),
                        IsClosed = lwpolyline.IsClosed,
                        Vertices = new List<PointEntity>(),
                        Centers = new List<PointEntity>(),
                        Angles = new List<PointEntity>(),
                        Radius = new List<float>(),
                        Type = "LwPolyLineEntity",
                        LayerId = layer.Id
                    };
                    foreach (var svgLine in svgPolyLine.Get())
                    {
                        lwpoly.Vertices.Add(new PointEntity
                        {
                            CapType = "Round",
                            X = svgLine.X,
                            Y = svgLine.Y,
                            Type = "PointEntity"
                        });
                    }
                    foreach (var center in svgPolyLine.GetCenters())
                    {
                        lwpoly.Centers.Add(new PointEntity
                        {
                            CapType = "Round",
                            X = center.X,
                            Y = center.Y,
                            Type = "PointEntity"
                        });
                    }
                    foreach (var angle in svgPolyLine.GetAngles())
                    {
                        lwpoly.Angles.Add(new PointEntity
                        {
                            CapType = "Round",
                            X = angle.X,
                            Y = angle.Y,
                            Type = "PointEntity"
                        });
                    }
                    foreach (var radious in svgPolyLine.GetRadious())
                    {
                        lwpoly.Radius.Add(radious);
                    }

                    GeometryEntities.Add(lwpoly);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "LwPolyLineEntity",
                        LayerId = layer.Id,
                        Data = lwpoly
                    });
                }
                else if (entity is ACadSharp.Entities.Insert insert)
                {
                    var insertScale = TransformHelper.GetScaleMatrix((float)insert.XScale, (float)insert.YScale);
                    var insertTranslate = TransformHelper.GetTranslateMatrix((float)insert.InsertPoint.X, (float)insert.InsertPoint.Y);
                    var insertRotation = TransformHelper.GetRotationMatrix((float)insert.Rotation);
                    var transformMatrix = insertTranslate * insertRotation * insertScale;
                    var totalMatrix = transform * transformMatrix;

                    var V = Vector<float>.Build;
                    var insertPoint = V.DenseOfArray(new float[] { (float)insert.InsertPoint.X, (float)insert.InsertPoint.Y, 1 });
                    insertPoint = transform * insertPoint;

                    var svgInsert = new InsertConverter2(insert, transform, mapScale, layer);
                    var converter = new EntityConverter(insert.Block.Entities.ToList(), layer, totalMatrix, mapScale, (float)insert.Rotation + rotation);
                    var insertEntity = new InsertEntity
                    {
                        CapType = "Round",
                        Entities = converter.Entities,
                        StrokeColor = svgInsert.GetStrokeColor(),
                        StrokeWidth = svgInsert.GetStrokeWidth(),
                        InsertPoint = new PointEntity
                        {
                            X = (float)insertPoint[0],
                            Y = (float)insertPoint[1]
                        },
                        Type = "InsertEntity",
                        LayerId = layer.Id
                    };
                    GeometryEntities.Add(insertEntity);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        EntityType = "InsertEntity",
                        LayerId = layer.Id,
                        Data = insertEntity
                    });
                }
                else if (entity is ACadSharp.Entities.Solid solid)
                {
                    var svgPolyLine = new SolidConverter(solid, transform, mapScale, layer);
                    var lwpolyColor = svgPolyLine.GetStrokeColor();
                    var lwpilyWidth = svgPolyLine.GetStrokeWidth();
                    var lwpoly = new LwPolyLineEntity
                    {
                        CapType = "Round",
                        StrokeColor = lwpolyColor,
                        StrokeWidth = lwpilyWidth,
                        DashArray = svgPolyLine.GetDashArray(),
                        IsClosed = true,
                        Vertices = new List<PointEntity>(),
                        Fill = lwpolyColor,
                        Type = "LwPolyLineEntity",
                        LayerId = layer.Id
                    };
                    foreach (var svgLine in svgPolyLine.Get())
                    {
                        lwpoly.Vertices.Add(new PointEntity
                        {
                            CapType = "Round",
                            X = svgLine.X,
                            Y = svgLine.Y,
                            Type = "PointEntity"
                        });
                    }
                    GeometryEntities.Add(lwpoly);
                    this.Entities.Add(new Models.Map.Entity
                    {
                        CapType = "Round",
                        StrokeColor = null,
                        StrokeWidth = null,
                        EntityType = "LwPolyLineEntity",
                        LayerId = layer.Id,
                        Data = lwpoly
                    });
                }
            }
        }
    }
}
