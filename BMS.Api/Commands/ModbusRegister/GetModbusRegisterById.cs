using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Roles.Extentions;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;
using BMS.Services.Modbus.Extentions;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class GetModbusRegisterByIdCommand : IRequest<QueryResultsModel>
    {
        public string RegisterId { get; set; }
    }

    // Command Handler
    public class GetModbusRegisterByIdCommandHandler : IRequestHandler<GetModbusRegisterByIdCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;

        public GetModbusRegisterByIdCommandHandler(IModbusRegisterService modbusRegisterService)
        {
            this.modbusRegisterService = modbusRegisterService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusRegisterByIdCommand request, CancellationToken cancellationToken)
        {
            var register = modbusRegisterService.GetById(request.RegisterId);
            if (register == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            register.IncludeModbus(modbusRegisterService);
            return new QueryResultsModel()
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = register.GetM()
            };
        }
    }
}
