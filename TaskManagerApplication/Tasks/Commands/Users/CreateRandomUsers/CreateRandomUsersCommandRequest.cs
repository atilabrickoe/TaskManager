using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers
{
    public class CreateRandomUsersCommandRequest : IRequest<CreateRandomUsersCommandResponse>
    {
        public int Amount { get; set; }
    }
}
