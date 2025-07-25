using MediatR;

namespace TaskManagerApplication.Tasks.Commands.AssociateTaskToUser
{
    public class AssociateTaskToUserComandRequest : IRequest<AssociateTaskToUserCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
    }
}
