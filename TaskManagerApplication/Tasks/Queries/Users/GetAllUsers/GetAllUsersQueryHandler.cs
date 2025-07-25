using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Queries.Users.GetAllUsers
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
                    Users = users.Select(u => UserDto.MapToDto(u)).ToList(),
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
                    Message = $"An error occurred while trying to retrieve users: {ex.Message}"
                };
            }
        }
    }
}
