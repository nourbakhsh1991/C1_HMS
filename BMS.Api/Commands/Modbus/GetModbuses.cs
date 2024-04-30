using MediatR;
using BMS.Api.Helpers;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Modbus.Interfaces;
using BMS.Modbus.Jobs;

namespace BMS.Api.Commands.Modbus
{
    // Command
    public class GetModbusesCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetModbusesCommandHandler : IRequestHandler<GetModbusesCommand, QueryResultsModel>
    {
        private readonly IModbusService modbusService;

        public GetModbusesCommandHandler(IModbusService modbusService)
        {
            this.modbusService = modbusService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusesCommand request, CancellationToken cancellationToken)
        {
            var entities = modbusService.GetAsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(a => (a.ServerAddress.Contains(request.QueryParam.filter) ||
                                                a.Name.ToLower().Contains(request.QueryParam.filter.ToLower())));
            // Sorting
            if (!string.IsNullOrEmpty(request.QueryParam.sortField))
                if (request.QueryParam.sortOrder?.ToLower() == "desc")
                    entities = entities.OrderByStrDescending(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                             request.QueryParam.sortField.Substring(1));
                else
                    entities = entities.OrderByStr(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                   request.QueryParam.sortField.Substring(1));
            else
                entities = entities.OrderByStr("UpdateTime");
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            // Including Features
            var result = entities.ToList().Select(a => a.GetM()).ToList();
            //foreach (var res in result)
            //{
            //    var connection = ModbusHandleJob.connections.FirstOrDefault(a =>
            //    a.Name == res.Name &&
            //    a.ServerAddress.ToString() == res.ServerAddress &&
            //    a.ServerPort == res.ServerPort);
            //    if (connection != null)
            //    {
            //        res.IsConnected = connection.IsStarted;
            //    }
            //}
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
