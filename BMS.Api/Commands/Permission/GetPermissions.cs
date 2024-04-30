using MediatR;
using BMS.Api.Helpers;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Permissions.Interfaces;
using BMS.Domain.Models.UserManagement;

namespace BMS.Api.Commands.Permission
{
    // Command
    public class GetPermissionsCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetPermissionsCommandHandler : IRequestHandler<GetPermissionsCommand, QueryResultsModel>
    {
        private readonly IPermissionService _permissionService;

        public GetPermissionsCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<QueryResultsModel> Handle(GetPermissionsCommand request, CancellationToken cancellationToken)
        {
            var entities = _permissionService.GetAsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(
                                a => (a.Name.ToLower().Contains(request.QueryParam.filter.ToLower()) ||
                                      a.Description.ToLower().Contains(request.QueryParam.filter.ToLower())));
            // Sorting
            if (!string.IsNullOrEmpty(request.QueryParam.sortField))
                if (request.QueryParam.sortOrder?.ToLower() == "desc")
                    entities = entities.OrderByStrDescending(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                             request.QueryParam.sortField.Substring(1));
                else
                    entities = entities.OrderByStr(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                   request.QueryParam.sortField.Substring(1));
            else
                entities = entities.OrderByStr("CreatedTime");
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            // Including Features
            var result = entities.ToList()
                            .Aggregate(new List<MPermission>(),
                                (list, current) =>
                                {
                                    list.Add(current.GetM());
                                    return list;
                                });
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = TotalCount,
                Result = result
            };
        }
    }
}
