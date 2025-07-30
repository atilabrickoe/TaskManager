using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;

namespace TaskManagerApplication.Users.Commands.CreateRandomUsers
{
    public class CreateRandomUsersCommandResponse : Response
    {
        public List<UserDto> Data { get; set; }
    }
}
