using MediatR;
using BMS.Api.Helpers;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Modbus.Interfaces;
using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Extentions;

namespace BMS.Api.Commands.ModbusHistory
{
    // Command
    public class GetModbusHistoriesCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetModbusHistoriesCommandHandler : IRequestHandler<GetModbusHistoriesCommand, QueryResultsModel>
    {
        private readonly IModbusHistoryService modbusHistoryService;

        public GetModbusHistoriesCommandHandler(IModbusHistoryService modbusHistoryService)
        {
            this.modbusHistoryService = modbusHistoryService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusHistoriesCommand request, CancellationToken cancellationToken)
        {
            var entities = modbusHistoryService.GetAsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(a => (a.Type.ToString().Contains(request.QueryParam.filter)));
            // Sorting
            if (!string.IsNullOrEmpty(request.QueryParam.sortField))
                if (request.QueryParam.sortOrder?.ToLower() == "desc")
                    entities = entities.OrderByStrDescending(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                             request.QueryParam.sortField.Substring(1));
                else
                    entities = entities.OrderByStr(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                   request.QueryParam.sortField.Substring(1));
            else
                entities = entities.OrderByStr("CreatedTime");
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            // Including Features
            var result = entities.ToList()
                .Aggregate(new List<MModbusEntityHistory>(),
                           (list, current) =>
                           {
                               var history = current.IncludeModbus(modbusHistoryService);
                               list.Add(history.GetM());
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
