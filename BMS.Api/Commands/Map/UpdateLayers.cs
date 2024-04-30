using BMS.Domain.ClientModels;
using BMS.Domain.Models.Map;
using BMS.Services.Map.Interfaces;
using BMS.Shared.Helpers;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class UpdateLayersCommand : IRequest<QueryResultsModel>
    {
        public string MLayersJson { get; set; }
    }

    // Command Handler
    public class UpdateLayersCommandHandler : IRequestHandler<UpdateLayersCommand, QueryResultsModel>
    {
        private readonly ILayerService _layerService;

        public UpdateLayersCommandHandler(ILayerService layerService)
        {
            _layerService = layerService;
        }

        public async Task<QueryResultsModel> Handle(UpdateLayersCommand request, CancellationToken cancellationToken)
        {
            var badRequest = new QueryResultsModel
            {
                Code = RequestResults.BadRequest.Code,
                Message = RequestResults.BadRequest.Message,
            };
            if (request.MLayersJson == null) return badRequest;
            var mLayers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MLayer>>(request.MLayersJson);
            if (mLayers == null || mLayers.Count == 0) return badRequest;
            var layers = mLayers.Select(a => a.GetBase()).ToList();
            await _layerService.Update(layers);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = mLayers.Count,
                Result = layers.Select(a => a.GetM()).ToList()
            };
        }
    }
}
