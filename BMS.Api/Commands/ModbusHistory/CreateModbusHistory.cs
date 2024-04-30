using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.ModbusHistory
{
    // Command
    public class CreateModbusHistoryCommand : IRequest<QueryResultsModel>
    {
        public ModbusEntityHistory History { get; set; }
    }

    // Command Handler
    public class CreateModbusHistoryCommandHandler : IRequestHandler<CreateModbusHistoryCommand, QueryResultsModel>
    {
        private readonly IModbusHistoryService modbusHistoryService;

        public CreateModbusHistoryCommandHandler(IModbusHistoryService modbusHistoryService)
        {
            this.modbusHistoryService = modbusHistoryService;
        }

        public async Task<QueryResultsModel> Handle(CreateModbusHistoryCommand request, CancellationToken cancellationToken)
        {
            if (request.History == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusHistoryService.Insert(request.History);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.History.Id
            };
        }
    }
}
