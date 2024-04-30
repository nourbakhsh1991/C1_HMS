using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Roles.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.Role
{
    // Command
    public class EditRoleCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.UserManagement.Role Role { get; set; }
    }

    // Command Handler
    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, QueryResultsModel>
    {
        private readonly IRoleService _roleService;

        public EditRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<QueryResultsModel> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.Role == null ||
                request.Role.Name.StartsWith("__") &&
                request.Role.Name.EndsWith("__"))
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await _roleService.Update(request.Role);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.Role.GetM()
            };
        }
    }
}
