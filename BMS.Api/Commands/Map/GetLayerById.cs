using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Map.Interfaces;
using BMS.Services.Map.Extensions;

namespace BMS.Api.Commands.Map
{
    // Query
    public class GetLayerByIdCommand : IRequest<QueryResultsModel>
    {
        public string LayerId { get; set; }
    }

    // Command Handler
    public class GetLayerByIdCommandHandler : IRequestHandler<GetLayerByIdCommand, QueryResultsModel>
    {
        private readonly ILayerService _layerService;

        public GetLayerByIdCommandHandler(ILayerService layerService)
        {
            _layerService = layerService;
        }

        public async Task<QueryResultsModel> Handle(GetLayerByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = _layerService.GetById(request.LayerId);
            if (entity == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            entity.IncludeEntities(_layerService);
            return new QueryResultsModel()
            {
                Count = 1,
                Result = entity.GetM(),
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
