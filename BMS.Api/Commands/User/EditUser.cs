using MediatR;
using BMS.Domain.ClientModels;
using BMS.Services.Users.Interfaces;
using BMS.Shared.Helpers;

namespace BMS.Api.Commands.User
{
    // Command
    public class EditUserCommand : IRequest<QueryResultsModel>
    {
        public Domain.Models.UserManagement.User User { get; set; }
    }

    // Command Handler
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, QueryResultsModel>
    {
        private readonly IUserService _userService;

        public EditUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var current = _userService.GetById(request.User.Id);
            if(current == null || !current.CanEdit)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.BadRequest.Code,
                    Message = RequestResults.BadRequest.Message
                };
            }
            request.User.Username = current.Username;
            request.User.Password = request.User.Password == null ? current.Password : CryptoService.CreateMD5(request.User.Password);
            await _userService.Update(request.User);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = request.User.GetM()
            };
        }
    }
}
