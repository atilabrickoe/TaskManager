using MediatR;

namespace TaskManagerApplication.Users.Commands.CreateRandomUsers
{
    public class CreateRandomUsersCommandRequest : IRequest<CreateRandomUsersCommandResponse>
    {
        public int Amount { get; set; }
    }
}
