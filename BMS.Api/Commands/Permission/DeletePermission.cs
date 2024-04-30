using MediatR;
using BMS.Services.Permissions.Interfaces;

namespace BMS.Api.Commands.Permission
{
    // Command
    public class DeletePermissionCommand : IRequest<Unit>
    {
        public string PermissionId { get; set; }
    }

    // Command Handler
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
    {
        private readonly IPermissionService _permissionService;

        public DeletePermissionCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            await _permissionService.Delete(request.PermissionId);
            return Unit.Value;
        }
    }
}
