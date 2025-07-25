using MediatR;

namespace TaskManagerApplication.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandRequest : IRequest<DeleteTaskCommandResponse>
    {
        public Guid TaskId { get; set; }
    }
}
