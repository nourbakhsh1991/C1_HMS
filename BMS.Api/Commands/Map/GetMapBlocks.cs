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
    public class GetMapBlocksCommand : IRequest<List<Domain.Models.Map.Block>>
    {
        public string MapId { get; set; }
    }

    // Command Handler
    public class GetMapBlocksCommandCommandHandler : IRequestHandler<GetMapBlocksCommand, List<Domain.Models.Map.Block>>
    {
        private readonly ILayerService _layerService;
        private readonly IMapService _mapService;
        private readonly IBlockService _blockService;

        public GetMapBlocksCommandCommandHandler(ILayerService layerService,
                                                IMapService mapService,
                                                IBlockService blockService)
        {
            _layerService = layerService;
            _mapService = mapService;
            _blockService = blockService;
        }

        public async Task<List<Domain.Models.Map.Block>> Handle(GetMapBlocksCommand request, CancellationToken cancellationToken)
        {

            
            var blocks = _blockService.GetAsQueryable().Where(a=>a.MapId == request.MapId).ToList();

            return blocks;
        }
    }
}
