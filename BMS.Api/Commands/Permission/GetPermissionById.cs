using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Permissions.Interfaces;

namespace BMS.Api.Commands.Permission
{
    // Command
    public class GetPermissionByIdCommand : IRequest<QueryResultsModel>
    {
        public string PermissionId { get; set; }
    }

    // Command Handler
    public class GetPermissionByIdCommandHandler : IRequestHandler<GetPermissionByIdCommand, QueryResultsModel>
    {
        private readonly IPermissionService _permissionService;

        public GetPermissionByIdCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<QueryResultsModel> Handle(GetPermissionByIdCommand request, CancellationToken cancellationToken)
        {
            // Loading Permission
            var entity = _permissionService.GetById(request.PermissionId);
            // If Null return Not Found
            if (entity == null)
            {
                return new QueryResultsModel
                {
                    Code = RequestResults.NotFound.Code,
                    Message = RequestResults.NotFound.Message
                };
            }
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
