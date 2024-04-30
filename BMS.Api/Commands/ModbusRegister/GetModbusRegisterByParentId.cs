using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Roles.Extentions;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;
using BMS.Services.Modbus.Extentions;
using Microsoft.Win32;

namespace BMS.Api.Commands.ModbusRegister
{
    // Command
    public class GetModbusRegisterByParentIdCommand : IRequest<QueryResultsModel>
    {
        public string ModbusId { get; set; }
    }

    // Command Handler
    public class GetModbusRegisterByParentIdCommandHandler : IRequestHandler<GetModbusRegisterByParentIdCommand, QueryResultsModel>
    {
        private readonly IModbusRegisterService modbusRegisterService;
        private readonly IModbusService modbusService;

        public GetModbusRegisterByParentIdCommandHandler(IModbusRegisterService modbusRegisterService,
                                                        IModbusService modbusService)
        {
            this.modbusRegisterService = modbusRegisterService;
            this.modbusService = modbusService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusRegisterByParentIdCommand request, CancellationToken cancellationToken)
        {
            var registers = modbusRegisterService.GetAsQueryable().Where(a => a.ModbusId == request.ModbusId).ToList();
            if (registers == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            if (registers.Count == 0)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            registers.IncludeModbus(modbusRegisterService);
            return new QueryResultsModel()
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = registers.Select(a => a.GetM())
            };
        }
    }
}
