using MediatR;
using Microsoft.AspNetCore.Mvc;
using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Permission;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.UserManagement;
using BMS.Shared.Helpers;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermissionController(IMediator mediator)
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
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessPermissions },
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
                var permissionsResult = await _mediator.Send(new GetPermissionsCommand { QueryParam = qp });
                if (permissionsResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(permissionsResult);
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
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessPermissions },
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
                var permissionResult = await _mediator.Send(new GetPermissionByIdCommand { PermissionId = id });
                if (permissionResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(permissionResult);
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
        public async Task<IActionResult> Post([FromBody] MPermission mPermission)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createPermission },
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
                var createPermissionResult = await _mediator.Send(new CreatePermissionCommand { Permission = mPermission.GetBase() });
                if (createPermissionResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(createPermissionResult);
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
        public async Task<IActionResult> Put([FromBody] MPermission mPermission)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createPermission },
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
                var editPermissionResult = await _mediator.Send(new EditPermissionCommand { Permission = mPermission.GetBase() });
                if (editPermissionResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(editPermissionResult);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createPermission },
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
                await _mediator.Send(new DeletePermissionCommand { PermissionId = id });
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Successful.Code,
                    Message = RequestResults.Successful.Message,
                    Count = 0
                });
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
