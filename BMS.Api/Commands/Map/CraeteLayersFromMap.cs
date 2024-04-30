using ACadSharp.IO;
using ACadSharp;
using BMS.Services.Map.Interfaces;
using MediatR;
using BMS.Shared.Common;
using BMS.Services.Map.Extensions;
using BMS.Shared.Helpers;
using MathNet.Numerics.LinearAlgebra;
using BMS.Domain.Converters;
using BMS.Domain.Geometry;
using BMS.Domain.Geometry.Point;
using BMS.Domain.Geometry.Insert;

namespace BMS.Api.Commands.Map
{
    // Query
    public class CreateLayerFromMapCommand : IRequest<List<string>>
    {
        public List<Domain.Models.Map.Layer> Layers { get; set; }
    }

    // Command Handler
    public class CreateLayerFromMapCommandHandler : IRequestHandler<CreateLayerFromMapCommand, List<string>>
    {
        private readonly ILayerService _layerService;
        private readonly IMapService _mapService;
        private readonly IGeometryService _geometryService;
        private readonly IBlockService _blockService;

        public CreateLayerFromMapCommandHandler(ILayerService layerService,
            IMapService mapService,
            IGeometryService geometryService,
            IBlockService blockService)
        {
            _layerService = layerService;
            _mapService = mapService;
            _geometryService = geometryService;
            _blockService = blockService;
        }

        public async Task<List<string>> Handle(CreateLayerFromMapCommand request, CancellationToken cancellationToken)
        {
            if (request.Layers == null || request.Layers.Count == 0)
                return new List<string>();

            var mapId = request.Layers[0].MapId;

            if (mapId == null) return new List<string>();

            var map = _mapService.GetById(mapId);

            CadDocument doc = DwgReader.Read(CommonPath.BaseDirectory + map.Path);

            var blocks = doc.BlockRecords.ToList();

            var layers = new List<Domain.Models.Map.Layer>();

            foreach (var layer in doc.Layers)
            {
                var clientLayer = request.Layers.FirstOrDefault(a => a.Name == layer.Name);
                if (clientLayer == null) continue;
                if (!clientLayer.Include) continue;
                layers.Add(new Domain.Models.Map.Layer
                {
                    StrokeColor = clientLayer.StrokeColor.HasValue ? clientLayer.StrokeColor.Value :
                                                                    (layer.Color.IsByLayer ? -1 :
                                                                    (layer.Color.IsByBlock ? -2 : (
                                                                    layer.Color.IsTrueColor ? layer.Color.TrueColor :
                                                                    layer.Color.Index))),
                    CreatedTime = DateTime.UtcNow,
                    Description = layer.ObjectName,
                    Name = clientLayer.Name,
                    LayerName = layer.Name,
                    DefaultPreview = clientLayer.DefaultPreview,
                    Include = clientLayer.Include,
                    MapId = mapId,
                    StrokeWidth = clientLayer.StrokeWidth,
                    UpdatedTime = DateTime.UtcNow,
                });
            }

            var bound = doc.GetBounds();

            var width = bound.Width;
            var height = bound.Height;
            var ratio = width / height;

            var maxSize = 2000;

            var shapeWidth = map.IsOriginalSize ? (width > height ? maxSize : maxSize * ratio) : map.Width;
            var shapeHeight = map.IsOriginalSize ? (width > height ? maxSize / ratio : maxSize) : map.Height;

            var M = Matrix<float>.Build;
            var V = Vector<float>.Build;

            var transformZero = TransformHelper.GetTranslateMatrix(-bound.X, -bound.Y);
            var flipCad = TransformHelper.GetScaleMatrix(1, -1);
            var transformCad = TransformHelper.GetTranslateMatrix(0, shapeHeight);
            //var scaleCad = TransformHelper.GetScaleMatrix(shapeWidth / width, shapeHeight / height);
            var scaleCad = TransformHelper.GetScaleMatrix(1, 1);
            var globalScale = transformCad * flipCad * scaleCad * transformZero;
            var globalSize = V.DenseOfArray(new float[] { shapeWidth, shapeHeight });
            //var mapScale = V.DenseOfArray(new float[] { shapeWidth / bound.Width, shapeHeight / bound.Height });
            var mapScale = V.DenseOfArray(new float[] { 1, 1 });

            var geometries = new List<Domain.Models.Map.Entity>();
            var blockGeometries = new List<Domain.Models.Map.Block>();


            var blockLayer = new ACadSharp.Tables.Layer("__BLOCKS__");
            var entityBlockLayer = new Domain.Models.Map.Layer
            {
                StrokeColor = 255,
                CreatedTime = DateTime.UtcNow,
                Description = blockLayer.ObjectName,
                Name = blockLayer.Name,
                LayerName = blockLayer.Name,
                DefaultPreview = false,
                Include = true,
                MapId = mapId,
                StrokeWidth = (int)LineweightType.W5,// (int)blockLayer.LineWeight, //StrokeHelper.GetLineWidth(blockLayer),
                UpdatedTime = DateTime.UtcNow,
            };

            foreach (ACadSharp.Tables.BlockRecord block in blocks)
            {
                if (block.Name.StartsWith("*")) continue;
                var blockConverter = new BlockConverter(block, globalScale, mapScale);
                var blockEntity = blockConverter.Get();
                var entities = block.Entities.ToList();
                var converter = new EntityConverter(entities, entityBlockLayer, M.DenseIdentity(3, 3), mapScale);
                blockEntity.Entities = converter.Entities;
                blockGeometries.Add(new Domain.Models.Map.Block
                {
                    CapType = "Round",
                    Data = blockEntity,
                    EntityType = "Block",
                    MapId = mapId,
                    Name = block.Name
                });
            }

            foreach (var layer in layers)
            {
                var entities = doc.Entities.Where(a => a.Layer.Name == layer.Name).ToList();
                var converter = new EntityConverter(entities, layer, globalScale, mapScale);
                geometries.AddRange(converter.Entities);
            }
            map.Width = (int)shapeWidth;
            map.Height = (int)shapeHeight;
            await _mapService.Update(map);
            await _geometryService.Insert(geometries);
            await _layerService.Insert(layers);
            await _blockService.Insert(blockGeometries);
            return layers.Select(a => a.Id).ToList();
        }
    }
}
