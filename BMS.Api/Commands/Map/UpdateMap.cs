using BMS.Domain.ClientModels;
using BMS.Services.Map.Interfaces;
using BMS.Shared.Helpers;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class UpdateMapCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.Map.Map Map { get; set; }
    }

    // Command Handler
    public class UpdateMapCommandHandler : IRequestHandler<UpdateMapCommand, QueryResultsModel>
    {
        private readonly IMapService _mapService;

        public UpdateMapCommandHandler(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<QueryResultsModel> Handle(UpdateMapCommand request, CancellationToken cancellationToken)
        {
            if (request.Map == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await _mapService.Update(request.Map);
            return new QueryResultsModel()
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = request.Map.GetM()
            };
        }
    }
}
