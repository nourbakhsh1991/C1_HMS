using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Modbus.Services
{
    public class BMSStreamService : BMSService.BMSServiceBase
    {
        public BMSStreamService() { }
        public override async Task Start(IAsyncStreamReader<BMSAuth> requestStream, IServerStreamWriter<BMSMessage> responseStream, ServerCallContext context)
        {
            //return base.Start(requestStream, responseStream, context);
            if (requestStream != null)
            {
                if (!await requestStream.MoveNext()) return;
            }
            try
            {
                if (!string.IsNullOrEmpty(requestStream.Current.Username))
                {
                   // TODO check user permissions;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
