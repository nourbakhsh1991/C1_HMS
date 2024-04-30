using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Roles.Extentions;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;

namespace BMS.Api.Commands.Modbus
{
    // Command
    public class GetModbusByIdCommand : IRequest<QueryResultsModel>
    {
        public string ModbusId { get; set; }
    }

    // Command Handler
    public class GetModbusByIdCommandHandler : IRequestHandler<GetModbusByIdCommand, QueryResultsModel>
    {
        private readonly IModbusService modbusService;

        public GetModbusByIdCommandHandler(IModbusService modbusService)
        {
            this.modbusService = modbusService;
        }

        public async Task<QueryResultsModel> Handle(GetModbusByIdCommand request, CancellationToken cancellationToken)
        {
            var modbus = modbusService.GetById(request.ModbusId);
            if (modbus == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            return new QueryResultsModel()
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = modbus.GetM()
            };
        }
    }
}
