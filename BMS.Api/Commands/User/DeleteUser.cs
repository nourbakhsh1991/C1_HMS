using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.User
{
    // Command
    public class DeleteUserCommand : IRequest<QueryResultsModel>
    {
        public string UserId { get; set; }
    }

    // Command Handler
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userService.GetById(request.UserId);
            if (user == null || !user.CanDelete)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            await _userService.Delete(request.UserId);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 0
            };
        }
    }
}
