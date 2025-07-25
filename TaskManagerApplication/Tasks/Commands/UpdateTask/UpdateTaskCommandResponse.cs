using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandResponse : Response
    {
        public TaskDto Task { get; set; }
    }
}
