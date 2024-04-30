using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.ModbusHistory
{
    // Command
    public class EditModbusHistoryCommand : IRequest<QueryResultsModel>
    {
        public ModbusEntityHistory History { get; set; }
    }

    // Command Handler
    public class EditModbusHistoryCommandHandler : IRequestHandler<EditModbusHistoryCommand, QueryResultsModel>
    {
        private readonly IModbusHistoryService modbusHistoryService;

        public EditModbusHistoryCommandHandler(IModbusHistoryService modbusHistoryService)
        {
            this.modbusHistoryService = modbusHistoryService;
        }

        public async Task<QueryResultsModel> Handle(EditModbusHistoryCommand request, CancellationToken cancellationToken)
        {
            if (request.History == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusHistoryService.Update(request.History);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = request.History.Id
            };
        }
    }
}
