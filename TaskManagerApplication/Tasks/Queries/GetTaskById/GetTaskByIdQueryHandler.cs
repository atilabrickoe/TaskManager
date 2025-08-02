using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQueryRequest, GetTaskByIdQueryResponse>
    {
        private readonly ITaskRepository _TaskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository TaskRepository)
        {
            _TaskRepository = TaskRepository;
        }

        public async Task<GetTaskByIdQueryResponse> Handle(GetTaskByIdQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var task = request.WithUser ? await _TaskRepository.GetByIdWithUserAsync(request.Id) :
                                              await _TaskRepository.GetByIdAsync(request.Id);
                if (task == null)
                    return new GetTaskByIdQueryResponse()
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                var response = new GetTaskByIdQueryResponse
                {
                    Success = true,
                    Data = TaskDto.MapToDto(task),
                };

                return response;
            }
            catch (Exception ex)
            {
                return new GetTaskByIdQueryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = $"Ocorreu um erro ao tentar recuperar tarefas: {ex.Message}"
                };
            }
        }
    }
}
