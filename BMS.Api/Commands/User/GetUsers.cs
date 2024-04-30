using MediatR;
using BMS.Api.Helpers;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Users.Interfaces;
using BMS.Services.Users.Extentions;

namespace BMS.Api.Commands.User
{
    // Command
    public class GetUsersCommand : IRequest<QueryResultsModel>
    {
        public QueryParamModel QueryParam { get; set; }
    }

    // Command Handler
    public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;

        public GetUsersCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(GetUsersCommand request, CancellationToken cancellationToken)
        {
            var entities = _userService.GetAsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(request.QueryParam.filter))
                entities = entities.Where(
                                a => (a.FirstName.ToLower().Contains(request.QueryParam.filter.ToLower()) ||
                                      a.LastName.ToLower().Contains(request.QueryParam.filter.ToLower()) ||
                                      a.Username.ToLower().Contains(request.QueryParam.filter.ToLower())));
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
                            .Aggregate(new List<MUser>(),
                                (list, current) =>
                                {
                                    var itm = current.IncludeRoles(_userService);
                                    list.Add(itm.GetM());
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
