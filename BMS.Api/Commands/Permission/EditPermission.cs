using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Permissions.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.Permission
{
    // Command
    public class EditPermissionCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.UserManagement.Permission Permission { get; set; }
    }

    // Command Handler
    public class EditPermissionCommandHandler : IRequestHandler<EditPermissionCommand, QueryResultsModel>
    {
        private readonly IPermissionService _permissionService;

        public EditPermissionCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<QueryResultsModel> Handle(EditPermissionCommand request, CancellationToken cancellationToken)
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
            await _permissionService.Update(request.Permission);
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
