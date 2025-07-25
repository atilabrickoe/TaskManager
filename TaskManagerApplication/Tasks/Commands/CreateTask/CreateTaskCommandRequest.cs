using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandRequest : IRequest<CreateTaskCommandResponse>
    {
        public CreateTaskDto Data { get; set; }
    }
}
