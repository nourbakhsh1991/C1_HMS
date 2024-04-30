using MediatR;
using BMS.Services.Permissions.Extentions;
using BMS.Services.Users.Interfaces;
using System.Security.Claims;

namespace BMS.Api.Commands.Authorization
{
    // Command
    public class GetAuthorizationCommand : IRequest<bool>
    {
        public List<string> AllRequiredPermissions { get; set; }
        public List<string> AnyRequiredPermissions { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }
    }

    // Command Handler
    public class GetAuthorizationCommandHandler : IRequestHandler<GetAuthorizationCommand, bool>
    {
        private readonly IUserService _userService;

        public GetAuthorizationCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(GetAuthorizationCommand request, CancellationToken cancellationToken)
        {
            var username = request.CurrentUser.Claims.FirstOrDefault(a => a.Type == "username");
            if (username == null)
            {
                return false;
            }
            var permissions = _userService.GetPermissionsByClaims(request.CurrentUser);
            if (!permissions.IsLoggedIn())
            {
                return false;
            }
            if (request.AllRequiredPermissions != null &&
                request.AllRequiredPermissions.Count != 0 &&
                !permissions.HasAll(request.AllRequiredPermissions))
            {
                return false;
            }
            if (request.AnyRequiredPermissions != null &&
                request.AnyRequiredPermissions.Count != 0 &&
                !permissions.HasAny(request.AnyRequiredPermissions))
            {
                return false;
            }
            return true;
        }
    }
}
