using MediatR;

namespace TaskManagerApplication.Tasks.Commands.AssociateTaskToUser
{
    public class AssociateTaskToUserCommandRequest : IRequest<AssociateTaskToUserCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
    }
}
