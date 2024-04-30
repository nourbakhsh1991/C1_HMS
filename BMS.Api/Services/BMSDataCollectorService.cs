using Grpc.Core;
using Microsoft.Win32;

namespace BMS.Api.Services
{
    public class BMSDataCollectorService : GrpcBMSDataCollectorService.GrpcBMSDataCollectorServiceBase
    {
        public BMSDataCollectorService() { }
        public override async Task Start(IAsyncStreamReader<BMSDataMessage> requestStream, IServerStreamWriter<Empty> responseStream, ServerCallContext context)
        {
            try
            {
                while (await requestStream.MoveNext())
                {
                    var currentData = requestStream.Current;
                    System.Diagnostics.Debug.WriteLine($"\tData {requestStream.Current.Data}");
                    await responseStream.WriteAsync(new Empty());
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public override Task<Empty> SendData(BMSDataMessage request, ServerCallContext context)
        {
            System.Diagnostics.Debug.WriteLine($"\tData {request.Data}");
            return Task.FromResult(new Empty());
        }
    }

    
}
