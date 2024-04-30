using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class CreateModbusRegisterCommand : IRequest<QueryResultsModel>
    {
        public ModbusEntityRegister Register { get; set; }
    }

    // Command Handler
    public class CreateModbusRegisterCommandHandler : IRequestHandler<CreateModbusRegisterCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;

        public CreateModbusRegisterCommandHandler(IModbusRegisterService modbusRegisterService)
        {
            this.modbusRegisterService = modbusRegisterService;
        }

        public async Task<QueryResultsModel> Handle(CreateModbusRegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Register == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusRegisterService.Insert(request.Register);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.Register.Id
            };
        }
    }
}
