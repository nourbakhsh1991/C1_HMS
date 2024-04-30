using MediatR;
using BMS.Services.Modbus.Interfaces;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class DeleteModbusRegisterCommand : IRequest<QueryResultsModel>
    {
        public string RegisterId { get; set; }
    }

    // Command Handler
    public class DeleteModbusRegisterCommandHandler : IRequestHandler<DeleteModbusRegisterCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;

        public DeleteModbusRegisterCommandHandler(IModbusRegisterService modbusRegisterService)
        {
            this.modbusRegisterService = modbusRegisterService;
        }

        public async Task<QueryResultsModel> Handle(DeleteModbusRegisterCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RegisterId))
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusRegisterService.Delete(request.RegisterId);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
