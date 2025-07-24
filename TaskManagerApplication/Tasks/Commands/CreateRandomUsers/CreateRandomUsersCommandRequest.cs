using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.CreateRandomUsers
{
    public class CreateRandomUsersCommandRequest : IRequest<CreateRandomUsersCommandResponse>
    {
        public CreateRandomUsersDto Data { get; set; }
    }
}
