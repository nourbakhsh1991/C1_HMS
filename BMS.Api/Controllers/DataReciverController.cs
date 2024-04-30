using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Map;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.Map;
using BMS.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataReciverController : ControllerBase
    {



        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DataSenderObjectType data)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\tData {data.Data}");
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = 1,
                });
            }
            catch (Exception ex)
            {
                return Ok(new QueryResultsModel()
                {
                    Code = RequestResults.InternalServerError.Code,
                    Message = RequestResults.InternalServerError.Message
                });
            }
        }


    }

    public class DataSenderObjectType
    {
        public string Data { get; set; }
    }
}
