using MediatR;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskComandRequest : IRequest<CreateTaskCommandResponse>
    {
        public CreateTaskDto Data { get; set; }
        public Guid LoggedUserId { get; set; }
    }
}
