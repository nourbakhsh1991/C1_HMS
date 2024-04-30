using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Map.Interfaces;
using BMS.Services.Map.Extensions;

namespace BMS.Api.Commands.Map
{
    // Query
    public class GetBlockByIdCommand : IRequest<QueryResultsModel>
    {
        public string BlockId { get; set; }
    }

    // Command Handler
    public class GetBlockByIdCommandHandler : IRequestHandler<GetBlockByIdCommand, QueryResultsModel>
    {
        private readonly IBlockService _blockService;

        public GetBlockByIdCommandHandler(IBlockService blockService)
        {
            _blockService = blockService;
        }

        public async Task<QueryResultsModel> Handle(GetBlockByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = _blockService.GetById(request.BlockId);
            if (entity == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }

            return new QueryResultsModel()
            {
                Count = 1,
                Result = entity,
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
