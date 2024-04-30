using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Map.Interfaces;
using BMS.Services.Map.Extensions;

namespace BMS.Api.Commands.Map
{
    // Query
    public class GetMapByIdCommand : IRequest<QueryResultsModel>
    {
        public string MapId { get; set; }
    }

    // Command Handler
    public class GetMapByIdCommandHandler : IRequestHandler<GetMapByIdCommand, QueryResultsModel>
    {
        private readonly IMapService _mapService;

        public GetMapByIdCommandHandler(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<QueryResultsModel> Handle(GetMapByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapService.GetById(request.MapId);
            if (entity == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            entity.IncludeLayers(_mapService);
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
