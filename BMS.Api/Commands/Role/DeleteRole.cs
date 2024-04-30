using MediatR;
using BMS.Services.Roles.Interfaces;

namespace BMS.Api.Commands.Role
{
    // Command
    public class DeleteRoleCommand : IRequest<Unit>
    {
        public string RoleId { get; set; }
    }

    // Command Handler
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
    {
        private readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleService.Delete(request.RoleId);
            return Unit.Value;
        }
    }
}
