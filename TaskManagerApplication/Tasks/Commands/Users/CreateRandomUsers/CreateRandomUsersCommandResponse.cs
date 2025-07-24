using TaskManagerDomain.Entities;

namespace TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers
{
    public class CreateRandomUsersCommandResponse : Response
    {
        public List<User> Users { get; set; }
    }
}
