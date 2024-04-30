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
    public class GetMapsCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetMapsCommandHandler : IRequestHandler<GetMapsCommand, QueryResultsModel>
    {
        private readonly IMapService _mapService;
        private readonly ILayerService _layerService;

        public GetMapsCommandHandler(IMapService mapService, ILayerService layerService)
        {
            _mapService = mapService;
            _layerService = layerService;
        }

        public async Task<QueryResultsModel> Handle(GetMapsCommand request, CancellationToken cancellationToken)
        {
            var entities = _mapService.GetAsQueryable();
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
            var result = entities.ToList()
                    .Aggregate(new List<MMap>(),
                        (list, current) =>
                        {
                            var mmap = current.IncludeLayers(_mapService);
                            if (mmap.Layers != null)
                                foreach (var layer in mmap.Layers)
                                {
                                    layer.IncludeEntities(_layerService);
                                }
                            list.Add(mmap.GetM());
                            return list;
                        });
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
