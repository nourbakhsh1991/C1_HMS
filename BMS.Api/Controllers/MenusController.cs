using MediatR;
using Microsoft.AspNetCore.Mvc;
using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Menus;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MenusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParamModel queryParam)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand { CurrentUser = User });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message,
                    });
                }
                var qp = queryParam == null ? new QueryParamModel() : queryParam;
                var menusResult = await _mediator.Send(new GetMenusCommand { QueryParam = qp, CurrentUser = User });
                if (menusResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(menusResult);
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand { CurrentUser = User });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message,
                    });
                }
                var allMenusResult = await _mediator.Send(new GetAllMenusCommand { CurrentUser = User });
                if (allMenusResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(allMenusResult);
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

        //[HttpGet("{id}")]
        //public IActionResult GetById(string id)
        //{
        //    try
        //    {
        //        var permissions = _userService.GetPermissionsByClaims(User);
        //        if (!permissions.IsLoggedIn())
        //        {
        //            return Ok(new QueryResultsModel
        //            {
        //                Code = RequestResults.Unauthorized.Code,
        //                Message = RequestResults.Unauthorized.Message,
        //            });
        //        }

        //        var entity = _menuService.GetById(id);
        //        if (!permissions.HasAny(entity.PermissionNames))
        //        {
        //            return Ok(new QueryResultsModel
        //            {
        //                Code = RequestResults.Unauthorized.Code,
        //                Message = RequestResults.Unauthorized.Message,
        //            });
        //        }

        //        return Ok(new QueryResultsModel()
        //        {
        //            Count = 1,
        //            Result = entity.GetM(),
        //            Code = RequestResults.Successful.Code,
        //            Message = RequestResults.Successful.Message
        //        });
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


        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] MAsideMenuItem value)
        //{
        //    try
        //    {
        //        // TOOD ' chech user permission'
        //        var station = value.GetAsideMenuItem();
        //        await _menuService.Insert(station);

        //        return Ok(new QueryResultsModel
        //        {
        //            Code = RequestResults.Created.Code,
        //            Message = RequestResults.Created.Message,
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

        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] MAsideMenuItem value)
        //{
        //    try
        //    {
        //        // TOOD ' chech user permission'
        //        var station = value.GetAsideMenuItem();
        //        await _menuService.Update(station);

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

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    try
        //    {
        //        // TOOD ' chech user permission'

        //        await _menuService.Delete(id);

        //        return Ok(new QueryResultsModel
        //        {
        //            Code = RequestResults.Successful.Code,
        //            Message = RequestResults.Successful.Message,
        //            Count = 0
        //        }); ;
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(new QueryResultsModel()
        //        {
        //            Code = RequestResults.InternalServerError.Code,
        //            Message = RequestResults.InternalServerError.Message
        //        });
        //    }

        //}
    }
}
