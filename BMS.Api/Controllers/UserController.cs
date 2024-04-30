using MediatR;
using Microsoft.AspNetCore.Mvc;
using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.User;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Helpers;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParamModel queryParam)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessUsers },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message
                    });
                }
                var qp = queryParam == null ? new QueryParamModel() : queryParam;
                var usersResult = await _mediator.Send(new GetUsersCommand { QueryParam = qp });
                if (usersResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(usersResult);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessUsers },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message
                    });
                }
                var userResult = await _mediator.Send(new GetUserByIdCommand { UserId = id });
                if (userResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(userResult);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MUser mUser)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createUser },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message
                    });
                }
                var createUserResult = await _mediator.Send(new CreateUserCommand { User = mUser.GetBase() });
                if (createUserResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(createUserResult);
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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MUser mUser)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createUser },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message
                    });
                }
                var editUserResult = await _mediator.Send(new EditUserCommand { User = mUser.GetBase() });
                if (editUserResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(editUserResult);
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

        //[HttpPut("password")]
        //public async Task<IActionResult> PutPassword([FromBody] MUser value)
        //{
        //    try
        //    {
        //        // TOOD ' chech user user'
        //        var user = value.GetBase();

        //        var current = _userService.GetById(user.Id);

        //        current.Password = user.Password;

        //        if (!current.CanEdit)
        //        {
        //            return Ok(new QueryResultsModel()
        //            {
        //                Code = RequestResults.BadRequest.Code,
        //                Message = RequestResults.BadRequest.Message
        //            });

        //        }

        //        await _userService.Update(current);

        //        return Ok(new QueryResultsModel
        //        {
        //            Code = RequestResults.Successful.Code,
        //            Message = RequestResults.Successful.Message,
        //            Count = 1,
        //            Result = value
        //        }); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new QueryResultsModel()
        //        {
        //            Code = RequestResults.InternalServerError.Code,
        //            Message = RequestResults.InternalServerError.Message
        //        });
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createUser },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message
                    });
                }
                var deleteUserResult = await _mediator.Send(new DeleteUserCommand { UserId = id });
                if (deleteUserResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(deleteUserResult);
            }
            catch (Exception)
            {
                return Ok(new QueryResultsModel()
                {
                    Code = RequestResults.InternalServerError.Code,
                    Message = RequestResults.InternalServerError.Message
                });
            }
        }
    }
}
