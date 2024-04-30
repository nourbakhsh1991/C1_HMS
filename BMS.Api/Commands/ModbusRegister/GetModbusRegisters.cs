using MediatR;
using BMS.Api.Helpers;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Modbus.Interfaces;
using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Extentions;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class GetModbusRegistersCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetModbusRegistersCommandHandler : IRequestHandler<GetModbusRegistersCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;

        public GetModbusRegistersCommandHandler(IModbusRegisterService modbusRegisterService)
        {
            this.modbusRegisterService = modbusRegisterService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusRegistersCommand request, CancellationToken cancellationToken)
        {
            var entities = modbusRegisterService.GetAsQueryable();
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
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            // Including Features
            var result = entities.ToList()
                .Aggregate(new List<MModbusEntityRegister>(),
                           (list, current) =>
                           {
                               var register = current.IncludeModbus(modbusRegisterService);
                               list.Add(register.GetM());
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
