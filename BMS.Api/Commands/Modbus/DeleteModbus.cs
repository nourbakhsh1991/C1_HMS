using MediatR;
using BMS.Services.Modbus.Interfaces;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.Modbus
{
    // Command
    public class DeleteModbusCommand : IRequest<QueryResultsModel>
    {
        public string ModbusId { get; set; }
    }

    // Command Handler
    public class DeleteModbusCommandHandler : IRequestHandler<DeleteModbusCommand, QueryResultsModel>
    {
        private readonly IModbusService modbusService;
        private readonly IModbusRegisterService registerService;

        public DeleteModbusCommandHandler(IModbusService modbusService,
                                          IModbusRegisterService registerService)
        {
            this.modbusService = modbusService;
            this.registerService = registerService;
        }

        public async Task<QueryResultsModel> Handle(DeleteModbusCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ModbusId))
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            var registers = registerService.GetByModbusId(request.ModbusId);
            if (registers != null && registers.Count > 0)
            {
                await registerService.Delete(registers);
            }
            await modbusService.Delete(request.ModbusId);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
