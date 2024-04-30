using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class EditModbusRegisterCommand : IRequest<QueryResultsModel>
    {
        public MModbusEntityRegister MRegister { get; set; }
    }

    // Command Handler
    public class EditModbusRegisterCommandHandler : IRequestHandler<EditModbusRegisterCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;

        public EditModbusRegisterCommandHandler(IModbusRegisterService modbusRegisterService)
        {
            this.modbusRegisterService = modbusRegisterService;
        }

        public async Task<QueryResultsModel> Handle(EditModbusRegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.MRegister == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusRegisterService.Update(request.MRegister.GetBase());
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = request.MRegister
            };
        }
    }
}
