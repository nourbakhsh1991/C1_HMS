using BMS.Domain.ClientModels;
using BMS.Domain.Models.Map;
using BMS.Services.Map.Interfaces;
using BMS.Shared.Helpers;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class DeleteLayersCommand : IRequest<QueryResultsModel>
    {
        public List<string> LayerIds { get; set; }
    }

    // Command Handler
    public class DeleteLayersCommandHandler : IRequestHandler<DeleteLayersCommand, QueryResultsModel>
    {
        private readonly ILayerService _layerService;

        public DeleteLayersCommandHandler(ILayerService layerService)
        {
            _layerService = layerService;
        }

        public async Task<QueryResultsModel> Handle(DeleteLayersCommand request, CancellationToken cancellationToken)
        {
            if (request.LayerIds == null || request.LayerIds.Count == 0)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message,
                };
            }
            await _layerService.Delete(request.LayerIds);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = request.LayerIds.Count,
                Result = request.LayerIds
            };
        }
    }
}
