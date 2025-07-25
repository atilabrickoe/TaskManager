using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.AssociateTaskToUser
{
    public class AssociateTaskToUserComandRequest : IRequest<AssociateTaskToUserCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
    }
}
