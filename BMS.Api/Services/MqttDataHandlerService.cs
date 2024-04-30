using BMS.Api.Commands.Modbus;
using BMS.Api.Commands.ModbusRegister;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.Mqtt;
using BMS.Api.Hubs;
using MediatR;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Win32;
using BMS.Domain.Models.Modbus;
using Svg;
using BMS.Modbus;
using BMS.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Api.Services
{
    public class MqttDataHandlerService : BackgroundService, IDisposable
    {
        private int executionCount = 0;
        private Timer? _timer = null;
        private IMqttClient _mqttClientPublisher;
        private IMqttClient _mqttClientSubscriber;
        private IMediator _mediator = null;
        private IServiceProvider _services;
        private IHubContext<ModbusDataHub> _dataHubContext = null;
        public MqttDataHandlerService(IServiceProvider services, IHubContext<ModbusDataHub> dataHubContext/*, IMediator mediator*/)
        {
            _services = services;
            _dataHubContext = dataHubContext;
            ConnectClient();
        }
        public async Task ConnectClient()
        {
            /*
             * This sample creates a simple MQTT client and connects to a public broker.
             *
             * Always dispose the client when it is no longer used.
             * The default version of MQTT is 3.1.1.
             */

            var mqttFactory = new MqttFactory();
            _mqttClientPublisher = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("localhost")
                .WithCleanSession()
                .Build();

            _mqttClientPublisher.ConnectedAsync += _mqttClientPublisher_ConnectedAsync;
            _mqttClientPublisher.DisconnectedAsync += _mqttClientPublisher_DisconnectedAsync;
            var response = await _mqttClientPublisher.ConnectAsync(options, CancellationToken.None);


            _mqttClientSubscriber = mqttFactory.CreateMqttClient();

            _mqttClientSubscriber.ConnectedAsync += _mqttClientSubscriber_ConnectedAsync;
            _mqttClientSubscriber.DisconnectedAsync += _mqttClientSubscriber_DisconnectedAsync;
            _mqttClientSubscriber.ApplicationMessageReceivedAsync += _mqttClientSubscriber_ApplicationMessageReceivedAsync;
            var response1 = await _mqttClientSubscriber.ConnectAsync(options, CancellationToken.None);

        }

        public async Task ReconnectClient(IMqttClient client)
        {
            if (client == null) return;


            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("localhost")
                .WithCleanSession()
                .Build();
            var response = await client.ConnectAsync(options, CancellationToken.None);
        }

        private async Task _mqttClientSubscriber_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            //System.Diagnostics.Debug.WriteLine(" * " + arg.ApplicationMessage.Topic);
            if (arg.ApplicationMessage.Topic == "rnBMS/getModbusList")
            {
                if (_mediator == null) return;

                var qp = new QueryParamModel();
                var modbuses = await _mediator.Send(new GetModbusesCommand { QueryParam = qp });
                if (modbuses != null && ((modbuses.Code / 100) == 2))
                {
                    var str = System.Text.Json.JsonSerializer.Serialize(modbuses.Result);
                    await PublishMessageAsync("rnBMS/setModbusList", str);
                }

            }
            else if (arg.ApplicationMessage.Topic == "rnBMS/getModbusRegisterList")
            {
                if (_mediator == null) return;

                var qp = new QueryParamModel();
                var registers = await _mediator.Send(new GetModbusRegistersCommand { QueryParam = qp });
                if (registers != null && ((registers.Code / 100) == 2))
                {
                    var str = System.Text.Json.JsonSerializer.Serialize(registers.Result);
                    await PublishMessageAsync("rnBMS/setModbusRegisterList", str);
                }

            }
            else if (arg.ApplicationMessage.Topic == "rnBMS/write")
            {
                var str = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
                var payload = System.Text.Json.JsonSerializer.Deserialize<RnMqttDataModel>(str);
                await _dataHubContext.Clients.All.SendAsync("NewData", payload);
                //System.Diagnostics.Debug.WriteLine(payload);
            }
            else if (arg.ApplicationMessage.Topic == "rnBMS/status/connect")
            {
                var str = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
                if (_mediator != null)
                {
                    var modbusResult = await _mediator.Send(new GetModbusByIdCommand { ModbusId = str });
                    if (modbusResult != null && modbusResult.Code == RequestResults.Successful.Code)
                    {
                        var modbus = modbusResult.Result as MModbusEntity;
                        if (modbus != null)
                        {
                            modbus.IsConnected = true;
                            await _mediator.Send(new EditModbusCommand { MModbus = modbus });
                        }
                    }
                }
                await _dataHubContext.Clients.All.SendAsync("ModbusConnected", str);
            }
            else if (arg.ApplicationMessage.Topic == "rnBMS/status/disconnect")
            {
                var str = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
                if (_mediator != null)
                {
                    var modbusResult = await _mediator.Send(new GetModbusByIdCommand { ModbusId = str });
                    if (modbusResult != null && modbusResult.Code == RequestResults.Successful.Code)
                    {
                        var modbus = modbusResult.Result as MModbusEntity;
                        if (modbus != null)
                        {
                            modbus.IsConnected = false;
                            await _mediator.Send(new EditModbusCommand { MModbus = modbus });
                        }
                    }
                }
                await _dataHubContext.Clients.All.SendAsync("ModbusDisconnected", str);
            }
        }

        private Task _mqttClientSubscriber_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task _mqttClientSubscriber_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            var filter = new MqttTopicFilterBuilder()
                            .WithTopic("rnBMS/write")
                            .Build();
            await _mqttClientSubscriber.SubscribeAsync(filter);

            filter = new MqttTopicFilterBuilder()
                            .WithTopic("rnBMS/getModbusList")
                            .Build();
            await _mqttClientSubscriber.SubscribeAsync(filter);

            filter = new MqttTopicFilterBuilder()
                            .WithTopic("rnBMS/getModbusRegisterList")
                            .Build();
            await _mqttClientSubscriber.SubscribeAsync(filter);

            filter = new MqttTopicFilterBuilder()
                            .WithTopic("rnBMS/status/connect")
                            .Build();
            await _mqttClientSubscriber.SubscribeAsync(filter);

            filter = new MqttTopicFilterBuilder()
                            .WithTopic("rnBMS/status/disconnect")
                            .Build();
            await _mqttClientSubscriber.SubscribeAsync(filter);
        }

        private Task _mqttClientPublisher_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task _mqttClientPublisher_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        public async Task<bool> SetRegister(ModbusEntityRegister register, int startIndex, List<int> values)
        {
            if (register == null) return false;
            if (values == null) return false;
            if ((startIndex) < 0 || (startIndex + values.Count) > register.Count) return false;
            if (register.Type == 2 || register.Type == 3) return false;
            RnMqttDataModel model = new RnMqttDataModel();
            model.Id = register.Id;
            model.StartAddress = startIndex;
            model.Count = values.Count;
            model.Data = values;
            model.Type = register.Type;
            var msg = System.Text.Json.JsonSerializer.Serialize(model);
            await PublishMessageAsync("rnBMS/read", msg);
            return true;
        }

        private async Task PublishMessageAsync(string topic, string message)
        {
            if (_mqttClientPublisher == null) return;
            //var count = Interlocked.Increment(ref executionCount);
            var mqttMessage = new MqttApplicationMessageBuilder()
                                .WithTopic(topic)
                                .WithPayload(message)
                                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                                .Build();
            if (_mqttClientPublisher.IsConnected)
            {
                await _mqttClientPublisher.PublishAsync(mqttMessage);
            }
            else
            {
                await ReconnectClient(_mqttClientPublisher);
            }
        }

        private async Task PublishModbusListAsync()
        {
            var modbusResult = await _mediator.Send(new GetModbusesCommand { QueryParam = new QueryParamModel() });
            var message = Newtonsoft.Json.JsonConvert.SerializeObject(modbusResult.Result);
            var mqttMessage = new MqttApplicationMessageBuilder()
                                .WithTopic("rnBMS/setModbusList")
                                .WithPayload(message)
                                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                                .Build();
            if (_mqttClientPublisher.IsConnected)
            {
                await _mqttClientPublisher.PublishAsync(mqttMessage);
            }
            else
            {
                await ReconnectClient(_mqttClientPublisher);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMilliseconds(500));

            return Task.CompletedTask;
        }
        private async void DoWork(object? state)
        {
            if (_mqttClientSubscriber != null && !_mqttClientSubscriber.IsConnected)
                await ReconnectClient(_mqttClientSubscriber);

            if (_mqttClientPublisher != null && !_mqttClientPublisher.IsConnected)
                await ReconnectClient(_mqttClientPublisher);
            if (_mediator == null)
            {
                var scope = _services.CreateScope();
                _mediator = scope.ServiceProvider.GetService<IMediator>();
            }
            if (_dataHubContext == null)
            {
                var scope = _services.CreateScope();
                //System.Diagnostics.Debug.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                _dataHubContext = scope.ServiceProvider.GetService<IHubContext<ModbusDataHub>>();
            }
            //await PublishMessageAsync();

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _mqttClientPublisher?.Dispose();
            _mqttClientSubscriber?.Dispose();
            _timer?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DoWork(stoppingToken);
            Task.Delay(500, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
