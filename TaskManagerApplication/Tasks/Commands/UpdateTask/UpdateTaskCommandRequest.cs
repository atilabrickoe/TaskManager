using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandRequest : IRequest<UpdateTaskCommandResponse>
    {
        public TaskDto Task { get; set; }
    }
}
