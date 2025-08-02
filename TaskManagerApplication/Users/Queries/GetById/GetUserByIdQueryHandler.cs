using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Users.Queries.GetById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.WithTask ? await _userRepository.GetByIdWithTaskAsync(request.Id) :
                                              await _userRepository.GetByIdAsync(request.Id);
                if (user == null)
                    return new GetUserByIdQueryResponse()
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.USER_NOT_FOUND
                    };
                var response = new GetUserByIdQueryResponse
                {
                    Success = true,
                    Data = UserDto.MapToDto(user),
                };

                return response;
            }
            catch (Exception ex)
            {
                return new GetUserByIdQueryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = $"Ocorreu um erro ao tentar recuperar usuários: {ex.Message}"
                };
            }
        }
    }
}
