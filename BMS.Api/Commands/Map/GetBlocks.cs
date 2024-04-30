using BMS.Domain.ClientModels;
using MediatR;
using BMS.Api.Helpers;
using BMS.Shared.Helpers;
using BMS.Services.Map.Interfaces;
using BMS.Domain.Models.Map;
using BMS.Services.Map.Extensions;

namespace BMS.Api.Commands.Map
{
    // Query
    public class GetBlocksCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetBlocksCommandHandler : IRequestHandler<GetBlocksCommand, QueryResultsModel>
    {
        private readonly IBlockService _blockService;
        private readonly ILayerService _layerService;

        public GetBlocksCommandHandler(IBlockService blockService, ILayerService layerService)
        {
            _blockService = blockService;
            _layerService = layerService;
        }

        public async Task<QueryResultsModel> Handle(GetBlocksCommand request, CancellationToken cancellationToken)
        {
            var entities = _blockService.GetAsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(
                                a => (a.Name.ToLower().Contains(request.QueryParam.filter.ToLower()))
                                     );
            // Sorting
            if (!string.IsNullOrEmpty(request.QueryParam.sortField))
                if (request.QueryParam.sortOrder?.ToLower() == "desc")
                    entities = entities.OrderByStrDescending(request.QueryParam.sortField.Substring(0, 1).ToUpper() + request.QueryParam.sortField.Substring(1));
                else
                    entities = entities.OrderByStr(request.QueryParam.sortField.Substring(0, 1).ToUpper() + request.QueryParam.sortField.Substring(1));
            else
                entities = entities.OrderByStr("CreatedTime");
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            var result = entities.ToList();

            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = TotalCount,
                Result = result
            };
        }
    }
}
