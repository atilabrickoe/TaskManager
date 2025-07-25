using MediatR;
namespace TaskManagerApplication.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQueryRequest : IRequest<GetAllTasksQueryResponse>
    {
        public bool WithUser { get; set; }
    }
}
