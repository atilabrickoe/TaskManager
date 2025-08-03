using MediatR;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommandRequest, UpdateTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository, INotificationService notificationService)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data.Id == Guid.Empty)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Id é obrigatório.",
                        Success = false,
                        ErrorCode = ErrorCodes.MISSING_INFORMATION
                    };
                }
                var existingTask = await _taskRepository.GetByIdAsync(request.Data.Id);
                if (existingTask == null)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Tarefa não encontrada.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                }

                existingTask.DueDate = request.Data.DueDate;
                existingTask.Status = request.Data.Status;
                existingTask.Title = request.Data.Title;
                existingTask.Description = request.Data.Description;

                if (request.Data.IdUser != null && request.Data.IdUser != Guid.Empty && request.Data.IdUser != existingTask.User?.Id)
                {
                    existingTask.User = await _userRepository.GetByIdAsync(request.Data.IdUser);
                }

                existingTask.IsValid();
                
                var updatedTask = await _taskRepository.UpdateAsync(existingTask);

                var response = new UpdateTaskCommandResponse()
                {
                    Message = "Tarefa atualizada com sucesso.",
                    Success = true,
                    Data = TaskDto.MapToDto(updatedTask)
                };
                await _notificationService.NotifyUserAsync(updatedTask.User.Id, $"Tarefa alterada, Responsavel:{updatedTask.User.UserName}, tarefa: " +
                    $"{updatedTask.Title}, data de vencimento: " +
                    $"{updatedTask.DueDate.ToString("dd/MM/yyyy")}");

                return response;
            }
            catch (WrongRequiredInformation ex)
            {
                return new UpdateTaskCommandResponse
                {
                    Message = ex.Message,
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION
                };
            }
            catch (Exception ex)
            {
                return new UpdateTaskCommandResponse
                {
                    Message = $"Ocorreu um erro ao atualizar a tarefa: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
