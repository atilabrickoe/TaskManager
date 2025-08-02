using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var users = request.WithTask ? await _userRepository.GetAllWithTaskAsync() :
                                               await _userRepository.GetAllAsync();
                var response = new GetAllUsersQueryResponse
                {
                    Data = users.Select(u => UserDto.MapToDto(u)).ToList(),
                    Success = true,
                };
                return response;
            }
            catch (Exception ex)
            {
                return new GetAllUsersQueryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = $"Ocorreu um erro ao tentar recuperar usuários: {ex.Message}"
                };
            }
        }
    }
}
