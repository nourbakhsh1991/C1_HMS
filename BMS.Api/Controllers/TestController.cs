using BMS.Api.Commands.ModbusRegister;
using BMS.Api.Services;
using BMS.Domain.Models.Modbus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly MqttDataHandlerService _mqttService;

        private readonly IMediator _mediator;
        public TestController(IServiceProvider serviceProvider, IMediator mediator)
        {
            _mqttService = serviceProvider.GetServices<IHostedService>().OfType<MqttDataHandlerService>().Single(); 
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var queryResults = await _mediator.Send(new GetModbusRegisterByIdCommand { RegisterId = id });
            var register = (queryResults.Result as MModbusEntityRegister)?.GetBase();
            if(register == null) return NotFound();
            var data = new List<int>();
            for (int i = 0;i< register.Count; i++)
            {
                data.Add(i * 2);
            }
            await _mqttService.SetRegister(register, 0, data);
            
            return Ok();
        }
    }
}
