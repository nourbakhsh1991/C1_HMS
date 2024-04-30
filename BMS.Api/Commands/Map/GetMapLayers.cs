using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Map.Interfaces;
using BMS.Services.Map.Extensions;
using ACadSharp.IO;
using ACadSharp;
using Svg;
using ACadSharp.Entities;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BMS.Api.Commands.Map
{
    // Query
    public class GetMapLayersCommand : IRequest<List<Domain.Models.Map.Layer>>
    {
        public string MapId { get; set; }
    }

    // Command Handler
    public class GetMapLayersCommandCommandHandler : IRequestHandler<GetMapLayersCommand, List<Domain.Models.Map.Layer>>
    {
        private readonly ILayerService _layerService;
        private readonly IMapService _mapService;

        public GetMapLayersCommandCommandHandler(ILayerService layerService,
                                                IMapService mapService)
        {
            _layerService = layerService;
            _mapService = mapService;
        }

        public async Task<List<Domain.Models.Map.Layer>> Handle(GetMapLayersCommand request, CancellationToken cancellationToken)
        {
            var map = _mapService.GetById(request.MapId);
            if (map == null)
            {
                return new List<Domain.Models.Map.Layer> { };
            }
            CadDocument doc = DwgReader.Read(CommonPath.BaseDirectory + map.Path);

            var layers = new List<Domain.Models.Map.Layer>();

            var layerColors = doc.Entities.Where(a => a.Color.IsByLayer).Select(a => new { layer = a.Layer, index = a.Color.Index }).DistinctBy(a => a.layer).ToList();

            foreach (var layer in doc.Layers)
            {
                var tmp = layerColors.FirstOrDefault(a => a.layer == layer);
                layers.Add(new Domain.Models.Map.Layer
                {
                    StrokeColor = tmp != null ? tmp.index : layer.Color.Index, // tmp != null ? ColorIndex.GetColorHex(ColorIndex.GetColorByIndex(tmp.index)) : ColorIndex.GetColorHex(ColorIndex.GetColorByIndex(layer.Color.Index)),
                    CreatedTime = DateTime.UtcNow,
                    Description = layer.ObjectName,
                    Name = layer.Name,
                    LayerName = layer.Name,
                    DefaultPreview = layer.IsOn,
                    Include = !layer.Flags.HasFlag(ACadSharp.Tables.LayerFlags.Frozen) && layer.IsOn,
                    MapId = request.MapId,
                    StrokeWidth = StrokeHelper.GetLineWidth(layer),
                    UpdatedTime = DateTime.UtcNow,
                }); ;
            }
            //await _layerService.Insert(layers);

            return layers;
        }
    }
}
