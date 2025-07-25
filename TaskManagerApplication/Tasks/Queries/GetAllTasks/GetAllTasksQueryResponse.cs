using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQueryResponse : Response
    {
        public List<TaskDto> Task { get; set; }
    }
}
