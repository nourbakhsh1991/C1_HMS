using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Modbus.Interfaces;
using BMS.Shared.Helpers;
using BMS.Domain.Models.Modbus;

namespace BMS.Api.Commands.Modbus
{
    // Command
    public class EditModbusCommand : IRequest<QueryResultsModel>
    {
        public MModbusEntity MModbus { get; set; }
    }

    // Command Handler
    public class EditModbusCommandHandler : IRequestHandler<EditModbusCommand, QueryResultsModel>
    {
        private readonly IModbusService modbusService;

        public EditModbusCommandHandler(IModbusService modbusService)
        {
            this.modbusService = modbusService;
        }

        public async Task<QueryResultsModel> Handle(EditModbusCommand request, CancellationToken cancellationToken)
        {
            if (request.MModbus == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await modbusService.Update(request.MModbus.GetBase());
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = request.MModbus
            };
        }
    }
}
