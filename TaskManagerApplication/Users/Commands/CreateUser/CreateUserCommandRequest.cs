using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Users.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
