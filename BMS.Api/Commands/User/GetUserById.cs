using MediatR;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using BMS.Services.Users.Extentions;
using BMS.Services.Users.Interfaces;

namespace BMS.Api.Commands.User
{
    // Command
    public class GetUserByIdCommand : IRequest<QueryResultsModel>
    {
        public string UserId { get; set; }
    }

    // Command Handler
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;

        public GetUserByIdCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            // Loading Role
            var entity = _userService.GetById(request.UserId);
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
            entity.IncludeRoles(_userService);
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
