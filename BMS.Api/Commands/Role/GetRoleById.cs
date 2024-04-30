using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Roles.Extentions;

namespace BMS.Api.Commands.Role
{
    // Command
    public class GetRoleByIdCommand : IRequest<QueryResultsModel>
    {
        public string RoleId { get; set; }
    }

    // Command Handler
    public class GetRoleByIdCommandHandler : IRequestHandler<GetRoleByIdCommand, QueryResultsModel>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<QueryResultsModel> Handle(GetRoleByIdCommand request, CancellationToken cancellationToken)
        {
            // Loading Role
            var entity = _roleService.GetById(request.RoleId);
            // If Null return Not Found
            if (entity == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
            // Including Features
            entity.IncludePermissions(_roleService);
            return new QueryResultsModel()
            {
                Count = 1,
                Result = entity.GetM(),
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }
    }
}
