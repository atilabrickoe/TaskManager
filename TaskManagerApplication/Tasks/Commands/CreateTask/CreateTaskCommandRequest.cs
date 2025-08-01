using MediatR;
using TaskManagerDomain.Dtos;
using Enum = TaskManagerDomain.Enums;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandRequest : IRequest<CreateTaskCommandResponse>
    {
        public TaskDto Data { get; set; }
    }
}
