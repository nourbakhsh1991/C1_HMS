using BMS.Api.Commands.ModbusRegister;
using BMS.Api.Services;
using BMS.Domain.Models.Modbus;
using BMS.Modbus.Jobs;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Api.Hubs
{
    public class ModbusDataHub : Hub<IRnHub>
    {
        private readonly MqttDataHandlerService _mqttService;

        private readonly IMediator _mediator;
        public ModbusDataHub(IServiceProvider serviceProvider, IMediator mediator)
        {
            _mqttService = serviceProvider.GetServices<IHostedService>().OfType<MqttDataHandlerService>().Single();
            _mediator = mediator;
        }
        public async Task SetData(string id, int startAddress, List<int> values)
        {
            //System.Diagnostics.Debug.WriteLine($" * {id} From Index : {index} One Value => {value} ");
            var queryResults = await _mediator.Send(new GetModbusRegisterByIdCommand { RegisterId = id });
            var register = (queryResults.Result as MModbusEntityRegister)?.GetBase();
            if (register == null) return;
            //var data = new List<int>() { value };
            await _mqttService.SetRegister(register, startAddress, values);

        }
    }
}
