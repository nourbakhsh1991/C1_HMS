using MediatR;
using Microsoft.IdentityModel.Tokens;
using BMS.Domain.BaseModels;
using BMS.Domain.ClientModels;
using BMS.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using BMS.Services.Users.Interfaces;
using BMS.Services.Users.Extentions;
using Microsoft.Extensions.Options;

namespace BMS.Api.Commands.Login
{
    // Command
    public class IsLoggedInCommand : IRequest<QueryResultsModel>
    {
        public string Token { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }
    }

    // Command Handler
    public class IsLoggedInCommandHandler : IRequestHandler<IsLoggedInCommand, QueryResultsModel>
    {
        private readonly AppAuth appAuth;
        private readonly IUserService _userService;

        public IsLoggedInCommandHandler(IOptions<AppAuth> _appAuth, IUserService userService)
        {
            appAuth = _appAuth.Value;
            _userService = userService;
        }

        public async Task<QueryResultsModel> Handle(IsLoggedInCommand request, CancellationToken cancellationToken)
        {
            var verified = true;
            if (string.IsNullOrEmpty(request.Token))
            {
                verified = false;
            }
            else
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(appAuth.Secret);
                    tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                }
                catch (Exception ex)
                {
                    verified = false;
                }
            }
            if (!verified)
            {
                // No User Logged In
                return new QueryResultsModel
                {
                    Code = RequestResults.Unauthorized.Code,
                    Message = RequestResults.Unauthorized.Message,
                };
            }
            var usernameClaim = request.CurrentUser.Claims.FirstOrDefault(a => a.Type == "username");
            var username = usernameClaim.Value;
            var user = _userService.GetByUsername(username);
            if (user == null)
                return new QueryResultsModel
                {
                    Code = RequestResults.Unauthorized.Code,
                    Message = RequestResults.Unauthorized.Message,
                };
            if (user.RoleIds == null || !user.RoleIds.Any())
                return new QueryResultsModel
                {
                    Code = RequestResults.Unauthorized.Code,
                    Message = RequestResults.Unauthorized.Message
                };
            user.IncludeRoles(_userService);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = user.GetM()
            };
        }
    }
}
