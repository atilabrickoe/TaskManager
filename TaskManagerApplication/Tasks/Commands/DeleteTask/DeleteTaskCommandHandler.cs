using MediatR;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommandRequest, DeleteTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<DeleteTaskCommandResponse> Handle(DeleteTaskCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _taskRepository.GetByIdWithUserAsync(request.TaskId);
                if (existingTask == null)
                {
                    return new DeleteTaskCommandResponse
                    {
                        Message = "Tarefa não encontrada.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                }

                await _taskRepository.DeleteAsync(existingTask.Id);

                var response = new DeleteTaskCommandResponse()
                {
                    Message = "Tarefa excluída com sucesso.",
                    Success = true
                };

                return response;
            }
            catch (Exception ex)
            {
                return new DeleteTaskCommandResponse
                {
                    Message = $"Ocorreu um erro ao criar a tarefa: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
