using MediatR;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.CreateRandomUsers
{
    public class CreateRandomUsersCommandHandler : IRequestHandler<CreateRandomUsersCommandRequest, CreateRandomUsersCommandResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateRandomUsersCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateRandomUsersCommandResponse> Handle(CreateRandomUsersCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var response = new CreateRandomUsersCommandResponse()
                {
                    Users = new List<User>()
                };

                for (int i = 0; i < request.Data.Amount; i++)
                {
                    var username = request.Data.UserNameMask.Replace("{{random}}", Guid.NewGuid().ToString("N").Substring(0, 8));
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = username,
                        PasswordHash = string.Empty // A ser definido posteriormente
                    };

                    user.IsValid();

                    response.Users.Add(await _userRepository.CreateAsync(user));
                }

                response.Success = true;

                return response;
            }
            catch (WrongRequiredInformation ex)
            {
                return new CreateRandomUsersCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new CreateRandomUsersCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.InternalServerError,
                    Message = ex.Message
                };
            }
        }
    }
}