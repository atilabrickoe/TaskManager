using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.TokenJwtAuthentication;
using TaskManagerApplication.Users.Commands.CreateUser;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public LoginCommandHandler(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.UserName);

                if (user == null)
                {
                    return new LoginCommandResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.USER_NOT_FOUND,
                        Message = $"User with Name {request.UserName} not found."
                    };
                }

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return new LoginCommandResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.LOGIN_WITH_WRONG_PASSWORD,
                        Message = $"Password is not valid for the user {user.UserName}."
                    };
                }


                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name , request.UserName!)
                };

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return new LoginCommandResponse()
                {
                    Success = true,
                    Token = new TokenJwt()
                    {
                        Accesstoken = jwt,
                        Expiration = token.ValidTo,
                        TokenType = "bearer",
                        UserId = user.Id,
                        UserName = user.UserName
                    }
                };
            }
            catch (WrongRequiredInformation ex)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new LoginCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = ex.Message
                };
            }
        }
    }
}