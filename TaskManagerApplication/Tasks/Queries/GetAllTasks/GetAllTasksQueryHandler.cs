using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQueryRequest, GetAllTasksQueryResponse>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<GetAllTasksQueryResponse> Handle(GetAllTasksQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = request.WithUser ? await _taskRepository.GetAllWithUserAsync() :
                                               await _taskRepository.GetAllAsync();
                var response = new GetAllTasksQueryResponse
                {
                    Task = tasks.Select(t => TaskDto.MapToDto(t)).ToList(),
                    Success = true,
                };
                return response;
            }
            catch (Exception ex)
            {
                return new GetAllTasksQueryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = $"An error occurred while trying to retrieve tasks: {ex.Message}"
                };
            }
        }
    }
}
