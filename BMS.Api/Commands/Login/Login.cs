using MediatR;
using Microsoft.IdentityModel.Tokens;
using BMS.Domain.BaseModels;
using BMS.Domain.ClientModels;
using BMS.Domain.Models.UserManagement;
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
    public class LoginCommand : IRequest<QueryResultsModel>
    {
        public LoginModel LoginModel { get; set; }
    }

    // Command Handler
    public class LoginCommandHandler : IRequestHandler<LoginCommand, QueryResultsModel>
    {
        private readonly AppAuth appAuth;
        private readonly IUserService _userService;

        public LoginCommandHandler(IOptions<AppAuth> _appAuth, IUserService userService)
        {
            _userService = userService;
            appAuth = _appAuth.Value;
        }

        public async Task<QueryResultsModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Models.UserManagement.User();
            user = _userService.GetAsQueryable().Where(
                    a =>
                        a.Username.ToLower() == request.LoginModel.username.ToLower() &&
                        a.Password.ToLower() == CryptoService.CreateMD5(request.LoginModel.password).ToLower()
                  ).FirstOrDefault();
            if (user == null)
            {
                return new QueryResultsModel()
                {
                    Code = RequestResults.Unauthorized.Code,
                    Message = RequestResults.Unauthorized.Message
                };
            }
            user.IncludeRoles(_userService);
            var permissions = user.GetPermissions();
            if (!permissions.Any(a => a.Name == DefaultSitePermissions.login)
                && !permissions.Any(a => a.Name == DefaultSitePermissions.admin))
                return new QueryResultsModel
                {
                    Code = RequestResults.Unauthorized.Code,
                    Message = RequestResults.Unauthorized.Message,
                    Count = 0
                };
            user.LastLogin = DateTime.UtcNow;
            await _userService.UpdateLogin(user);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("username", user.Username),
                    new Claim("token", "")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appAuth.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDesc);
            var token = tokenHandler.WriteToken(securityToken);
            return new QueryResultsModel
            {
                Code = RequestResults.Successful.Code,
                Message = RequestResults.Successful.Message,
                Count = 1,
                Result = new
                {
                    token,
                    username = user.Username
                }
            };
        }
    }
}
