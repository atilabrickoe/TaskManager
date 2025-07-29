using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Users.Commands.CreateUser
{
    public class CreateUserCommandResponse : Response
    {
        public UserDto User { get; set; }
    }
}
