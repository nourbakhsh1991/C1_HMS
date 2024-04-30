using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using System.Security.Claims;
using BMS.Services.Users.Interfaces;
using BMS.Services.Permissions.Extentions;
using BMS.Services.Menus.Interfaces;
using BMS.Api.Helpers;
using BMS.Services.Modbus.Interfaces;

namespace BMS.Api.Commands.Menus
{
    // Command
    public class GetMenusCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }
    }

    // Command Handler
    public class GetMenusCommandHandler : IRequestHandler<GetMenusCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        public GetMenusCommandHandler(IMenuService menuService, IUserService userService)
        {
            _userService = userService;
            _menuService = menuService;
        }

        public async Task<QueryResultsModel> Handle(GetMenusCommand request, CancellationToken cancellationToken)
        {
            var permissions = _userService.GetPermissionsByClaims(request.CurrentUser);
            var entities = _menuService.GetAsQueryable()
                                .Where(a => true /*permissions.HasAny(a.PermissionNames)*/);
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(a => a.Name.Contains(request.QueryParam.filter));
            // Sorting
            if (!string.IsNullOrEmpty(request.QueryParam.sortField))
                if (request.QueryParam.sortOrder?.ToLower() == "desc")
                    entities = entities.OrderByStrDescending(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                             request.QueryParam.sortField.Substring(1));
                else
                    entities = entities.OrderByStr(request.QueryParam.sortField.Substring(0, 1).ToUpper() +
                                                   request.QueryParam.sortField.Substring(1));
            // Paging
            var skip = (request.QueryParam.pageNumber - 1) * request.QueryParam.pageSize;
            var TotalCount = entities.Count();
            if (TotalCount > request.QueryParam.pageSize)
                entities = entities.Skip(skip).Take(request.QueryParam.pageSize);
            // Retrive the data
            // TODO 'maybe change to a model'
            var result = entities.ToList().Select(a => a.GetM());
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
