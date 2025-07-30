using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.AssociateTaskToUser
{
    public class AssociateTaskToUserCommandResponse : Response
    {
        public TaskDto Data { get; set; }
    }
}
