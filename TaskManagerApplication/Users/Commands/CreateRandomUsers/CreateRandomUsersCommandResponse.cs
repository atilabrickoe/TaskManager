using TaskManagerDomain.Entities;

namespace TaskManagerApplication.Users.Commands.CreateRandomUsers
{
    public class CreateRandomUsersCommandResponse : Response
    {
        public List<User> Users { get; set; }
    }
}
