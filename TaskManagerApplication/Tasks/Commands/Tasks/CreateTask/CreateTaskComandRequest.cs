using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.CreateTask
{
    public class CreateTaskComandRequest : IRequest<CreateTaskCommandResponse>
    {
        public CreateTaskDto Data { get; set; }
        public Guid UserId { get; set; }
    }
}
