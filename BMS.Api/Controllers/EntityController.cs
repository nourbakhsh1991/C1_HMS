using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Map;
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
    public class EntityController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMediator _mediator;
        public EntityController(IFileService fileService, IMediator mediator)
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
                var productsResult = await _mediator.Send(new GetMapsCommand { QueryParam = qp });
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
                var productResult = await _mediator.Send(new GetMapByIdCommand { MapId = id });
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

        [HttpGet("layers/{id}")]
        public async Task<IActionResult> GetLayersAsync(string id)
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
                var layersResult = await _mediator.Send(new GetMapLayersCommand { MapId = id });
                if (layersResult == null)
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
                    Count = layersResult.Count,
                    Result = layersResult.Select(a => a.GetM()).ToList()
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

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm(Name = "map")] string mapJson, [FromForm(Name = "file")] IFormFile? file)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createMap },
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
                var mMap = Newtonsoft.Json.JsonConvert.DeserializeObject<MMap>(mapJson);
                if (mMap == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }

                if (file == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                mMap.Path = _fileService.SaveFile(file, Directory.GetCurrentDirectory());
                var productId = await _mediator.Send(new CreateMapCommand { Map = mMap.GetBase() });
                mMap.Id = productId;
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = 1,
                    Result = mMap
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

        [HttpPost("createState")]
        public async Task<IActionResult> PostCreateStateAsync(State state)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createMap },
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

                var entityId = await _mediator.Send(new CreateStateCommand { State = state });
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = 1,
                    Result = entityId
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

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Entity entity)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.createMap },
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
                if (entity == null || entity.Data == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }

                var entityId = await _mediator.Send(new UpdateEntityCommand { Entity = entity });

                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Successful.Code,
                    Message = RequestResults.Successful.Message,
                    Count = 1,
                    Result = entity
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
