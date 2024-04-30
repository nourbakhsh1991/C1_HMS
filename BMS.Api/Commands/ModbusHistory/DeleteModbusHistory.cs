using MediatR;
using BMS.Services.Modbus.Interfaces;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.ModbusHistory
{
    // Command
    public class DeleteModbusHistoryCommand : IRequest<QueryResultsModel>
    {
        public string HistoryId { get; set; }
    }

    // Command Handler
    public class DeleteModbusHistoryCommandHandler : IRequestHandler<DeleteModbusHistoryCommand, QueryResultsModel>
    {
        private readonly IModbusHistoryService modbusHistoryService;

        public DeleteModbusHistoryCommandHandler(IModbusHistoryService modbusHistoryService)
        {
            this.modbusHistoryService = modbusHistoryService;
        }

        public async Task<QueryResultsModel> Handle(DeleteModbusHistoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.HistoryId))
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusHistoryService.Delete(request.HistoryId);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
