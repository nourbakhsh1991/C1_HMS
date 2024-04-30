using MediatR;
using Microsoft.AspNetCore.Mvc;
using BMS.Api.Commands.Login;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Helpers;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var token = "";
                if (Request.Headers.ContainsKey("Authorization"))
                    token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                var isLoggedInResult = await _mediator.Send(new IsLoggedInCommand { Token = token, CurrentUser = User });
                if (isLoggedInResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message,
                    });
                }
                return Ok(isLoggedInResult);
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

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel loginModel)
        {
            var loginResult = await _mediator.Send(new LoginCommand { LoginModel = loginModel });
            if (loginResult == null)
            {
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message

                });
            }
            return Ok(loginResult);
        }
    }
}
