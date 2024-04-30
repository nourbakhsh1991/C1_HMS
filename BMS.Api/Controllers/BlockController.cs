using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Map;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.Map;
using BMS.Services.Files.Interfaces;
using BMS.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMediator _mediator;
        public BlockController(IFileService fileService, IMediator mediator)
        {
            _fileService = fileService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] QueryParamModel queryParam)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessMaps },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message,
                    });
                }
                var qp = queryParam == null ? new QueryParamModel() : queryParam;
                var productsResult = await _mediator.Send(new GetBlocksCommand { QueryParam = qp });
                if (productsResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(productsResult);
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
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessMaps },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message,
                    });
                }
                var productResult = await _mediator.Send(new GetBlockByIdCommand { BlockId = id });
                if (productResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(productResult);
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

        [HttpGet("map/{id}")]
        public async Task<IActionResult> GetMapBlocksAsync(string id)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.accessMaps },
                    CurrentUser = User
                });
                if (!isAuthorized)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.Unauthorized.Code,
                        Message = RequestResults.Unauthorized.Message,
                    });
                }
                var blocksResult = await _mediator.Send(new GetMapBlocksCommand { MapId = id });
                if (blocksResult == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(new QueryResultsModel()
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = blocksResult.Count,
                    Result = blocksResult
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
}
