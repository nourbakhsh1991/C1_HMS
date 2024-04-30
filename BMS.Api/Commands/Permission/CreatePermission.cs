using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Permissions.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.Permission
{
    // Command
    public class CreatePermissionCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.UserManagement.Permission Permission { get; set; }
    }

    // Command Handler
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, QueryResultsModel>
    {
        private readonly IPermissionService _permissionService;

        public CreatePermissionCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<QueryResultsModel> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            if (request.Permission == null ||
                request.Permission.Name.StartsWith("__") &&
                request.Permission.Name.EndsWith("__"))
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await _permissionService.Insert(request.Permission);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.Permission.GetM()
            };
        }
    }
}
