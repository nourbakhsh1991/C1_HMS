using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using System.Security.Claims;
using BMS.Services.Users.Interfaces;
using BMS.Services.Menus.Interfaces;
using BMS.Services.Permissions.Extentions;
using BMS.Domain.Menues;
using BMS.Services.Modbus.Interfaces;

namespace BMS.Api.Commands.Menus
{
    // Command
    public class GetAllMenusCommand : IRequest<QueryResultsModel>
    {
        public ClaimsPrincipal CurrentUser { get; set; }
    }

    // Command Handler
    public class GetAllMenusCommandHandler : IRequestHandler<GetAllMenusCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        public GetAllMenusCommandHandler(IMenuService menuService, IUserService userService)
        {
            _userService = userService;
            _menuService = menuService;
        }

        public async Task<QueryResultsModel> Handle(GetAllMenusCommand request, CancellationToken cancellationToken)
        {
            var permissions = _userService.GetPermissionsByClaims(request.CurrentUser);
            var entity = _menuService.GetAsQueryable().ToList()
                    .Where(a => a.PermissionNames == null || permissions.HasAny(a.PermissionNames))
                    .ToList();
            var result = entity.ToList();
            merge(entity, entity, result);
            return new QueryResultsModel()
            {
                Count = result.Count,
                Result = result.Select(a => a.GetM()),
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message
            };
        }

        private List<AsideMenuItem> merge(List<AsideMenuItem> items, List<AsideMenuItem> all, List<AsideMenuItem> result)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.HasSubMenu) continue;
                if (item.SubMenuIds != null && item.SubMenus != null && item.SubMenus.Count > 0) continue;
                var childs = new List<AsideMenuItem>();
                getChilds(item, childs, all);
                var itemChilds = childs.Where(a => item.SubMenuIds.Contains(a.Id));
                item.SubMenus = itemChilds.ToList();
                childs.RemoveAll(a => item.SubMenuIds.Contains(a.Id));
                result.RemoveAll(a => item.SubMenuIds.Contains(a.Id));
                merge(item.SubMenus, childs, result);
                i = -1;
            }
            return items;
        }

        private void getChilds(AsideMenuItem item, List<AsideMenuItem> childs, List<AsideMenuItem> all)
        {
            var ch = all.Where(a => item.SubMenuIds.Contains(a.Id));
            childs.AddRange(ch);
            foreach (var child in ch)
            {
                getChilds(child, childs, all);
            }
        }
    }
}
