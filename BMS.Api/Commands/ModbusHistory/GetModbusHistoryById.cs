using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Roles.Extentions;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;
using BMS.Services.Modbus.Extentions;

namespace BMS.Api.Commands.ModbusHistory
{
    // Command
    public class GetModbusHistoryByIdCommand : IRequest<QueryResultsModel>
    {
        public string HistoryId { get; set; }
    }

    // Command Handler
    public class GetModbusHistoryByIdCommandHandler : IRequestHandler<GetModbusHistoryByIdCommand, QueryResultsModel>
    {
        private readonly IModbusHistoryService modbusHistoryService;

        public GetModbusHistoryByIdCommandHandler(IModbusHistoryService modbusHistoryService)
        {
            this.modbusHistoryService = modbusHistoryService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusHistoryByIdCommand request, CancellationToken cancellationToken)
        {
            var history = modbusHistoryService.GetById(request.HistoryId);
            if (history == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            history.IncludeModbus(modbusHistoryService);
            return new QueryResultsModel()
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = history.GetM()
            };
        }
    }
}
