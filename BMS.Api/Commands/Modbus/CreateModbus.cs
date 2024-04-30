using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.Modbus
{
    // Command
    public class CreateModbusCommand : IRequest<QueryResultsModel>
    {
        public ModbusEntity Modbus { get; set; }
    }

    // Command Handler
    public class CreateModbusCommandHandler : IRequestHandler<CreateModbusCommand, QueryResultsModel>
    {
        private readonly IModbusService modbusService;

        public CreateModbusCommandHandler(IModbusService modbusService)
        {
            this.modbusService = modbusService;
        }

        public async Task<QueryResultsModel> Handle(CreateModbusCommand request, CancellationToken cancellationToken)
        {
            if (request.Modbus == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusService.Insert(request.Modbus);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.Modbus
            };
        }
    }
}
