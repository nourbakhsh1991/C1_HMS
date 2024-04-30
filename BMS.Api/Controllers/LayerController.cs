using BMS.Api.Commands.Authorization;
using BMS.Api.Commands.Map;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.Map;
using BMS.Services.Files.Interfaces;
using BMS.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayerController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMediator _mediator;
        public LayerController(IFileService fileService, IMediator mediator)
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
                var productsResult = await _mediator.Send(new GetLayersCommand { QueryParam = qp });
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
                var productResult = await _mediator.Send(new GetLayerByIdCommand { LayerId = id });
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

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MLayer mLayer)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.editLayers },
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
                if (mLayer == null)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                var layerId = await _mediator.Send(new CreateLayerCommand { Layer = mLayer.GetBase() });
                mLayer.Id = layerId;
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = 1,
                    Result = mLayer
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

        [HttpPost("createFromMap")]
        public async Task<IActionResult> PostCreateFromMapAsync([FromBody] List<MLayer> mLayer)
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
                if (mLayer == null || mLayer.Count == 0)
                {
                    return Ok(new QueryResultsModel()
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                var layerIds = await _mediator.Send(new CreateLayerFromMapCommand { Layers = mLayer.Select(a => a.GetBase()).ToList() });
                return Ok(new QueryResultsModel
                {
                    Code = RequestResults.Created.Code,
                    Message = RequestResults.Created.Message,
                    Count = layerIds.Count,
                    Result = layerIds
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
        public async Task<IActionResult> PutLayersAsync([FromForm(Name = "layersJson")] string mLayersJson)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.editLayers },
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
                var updateResult = await _mediator.Send(new UpdateLayersCommand { MLayersJson = mLayersJson });
                if (updateResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(updateResult);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteLayersAsync([FromBody] List<string> layerIds)
        {
            try
            {
                var isAuthorized = await _mediator.Send(new GetAuthorizationCommand
                {
                    AllRequiredPermissions = new List<string> { DefaultSitePermissions.editLayers },
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
                var deleteResult = await _mediator.Send(new DeleteLayersCommand { LayerIds = layerIds });
                if (deleteResult == null)
                {
                    return Ok(new QueryResultsModel
                    {
                        Code = RequestResults.BadRequest.Code,
                        Message = RequestResults.BadRequest.Message
                    });
                }
                return Ok(deleteResult);
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
