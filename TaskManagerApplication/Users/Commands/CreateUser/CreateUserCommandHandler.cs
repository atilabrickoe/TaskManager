using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerApplication.Users.Commands.CreateRandomUsers;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var userAlreadyExists = await _userRepository.GetByUsernameAsync(request.UserName);
                if (userAlreadyExists != null)
                {
                    return new CreateUserCommandResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.USER_ALREADY_EXISTS,
                        Message = $"Usuário com nome {request.UserName} já existe."
                    };
                }

                var hasher = new PasswordHasher<User>();
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName
                };
                user.PasswordHash = hasher.HashPassword(user, request.Password);

                await _userRepository.CreateAsync(user);

                var response = new CreateUserCommandResponse();
                response.Data = UserDto.MapToDto(user);
                response.Success = true;

                return response;
            }
            catch (WrongRequiredInformation ex)
            {
                return new CreateUserCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new CreateUserCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = ex.Message
                };
            }
        }
    }
}