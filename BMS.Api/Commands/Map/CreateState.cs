using ACadSharp.Entities;
using BMS.Domain.Converters;
using BMS.Domain.Geometry.Arc;
using BMS.Domain.Geometry.Block;
using BMS.Domain.Geometry.Circle;
using BMS.Domain.Geometry.Insert;
using BMS.Domain.Geometry.Line;
using BMS.Services.Map.Interfaces;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using MediatR;
using Svg;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BMS.Api.Commands.Map
{
    // Query
    public class CreateStateCommand : IRequest<string>
    {
        public Domain.Models.Map.State State { get; set; }
    }

    // Command Handler
    public class CreateStateCommandHandler : IRequestHandler<CreateStateCommand, string>
    {
        private readonly IMapService _mapService;
        private readonly IBlockService _blockService;
        private readonly IGeometryService _geometryService;
        private readonly ILayerService _layerService;

        public CreateStateCommandHandler(IMapService mapService,
            IBlockService blockService,
            IGeometryService geometryService,
            ILayerService layerService)
        {
            _mapService = mapService;
            _blockService = blockService;
            _geometryService = geometryService;
            _layerService = layerService;
        }

        public async Task<string> Handle(CreateStateCommand request, CancellationToken cancellationToken)
        {
            var block = _blockService.GetById(request.State.BlockId);
            var geometry = _geometryService.GetById(request.State.ObjectId);
            var layer = _layerService.GetById(geometry.LayerId);
            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            var transformCad = TransformHelper.GetTranslateMatrix(request.State.TranslateX, request.State.TranslateY);
            var scaleCad = TransformHelper.GetScaleMatrix(request.State.ScaleX,request.State.ScaleY);
            var rotationCad = TransformHelper.GetRotationMatrix(request.State.Rotation);
            var transformZero = TransformHelper.GetTranslateMatrix(-request.State.Bound.X, -request.State.Bound.Y);
            var transformPivot = TransformHelper.GetTranslateMatrix(-request.State.Bound.Width / 2, -request.State.Bound.Height / 2);
            var transform = transformCad * scaleCad * rotationCad * transformZero * transformPivot;
            var flip = MathF.Sign(scaleCad[0, 0]) * MathF.Sign(scaleCad[1, 1]) < 0;
            var blockEntity = block.Data as BlockEntity;
            var entities = blockEntity.Entities.ToList();
            foreach( var entity in entities )
            {
                if (entity.Data is LineEntity line)
                {
                    line.UpdateTransform(transform);
                }
                else if (entity.Data is ArcEntity arc)
                {
                    arc.UpdateTransform(transform , flip);
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
                else if (entity.Data is InsertEntity insert)
                {
                    insert.UpdateTransform(transform);
                }
            }
            if (geometry.StateData == null) geometry.StateData = new List<object>();
            geometry.StateData.Add(entities);

            await _geometryService.Update(geometry);

            return geometry.Id;
        }
    }
}
