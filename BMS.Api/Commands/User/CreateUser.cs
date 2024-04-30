using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.User
{
    // Command
    public class CreateUserCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.UserManagement.User User { get; set; }
    }

    // Command Handler
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.User.Username == null || request.User.Password == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = "Username And Password Cannot Be Empty"
                };
            }
            if (request.User.Username.ToLower() == "admin")
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            var usernameExist = _userService.GetAsQueryable().Any(a => a.Username == request.User.Username);
            if (usernameExist)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = "Username Allready Exist."
                };
            }
            request.User.Password = CryptoService.CreateMD5(request.User.Password);
            await _userService.Insert(request.User);
            return new QueryResultsModel
            {
                Code = RequestResults.Created.Code,
                Message = RequestResults.Created.Message,
                Count = 1,
                Result = request.User.GetM()
            };
        }
    }
}
