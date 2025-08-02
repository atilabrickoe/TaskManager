using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.AssociateTaskToUser
{
    public class AssociateTaskToUserCommandHandler : IRequestHandler<AssociateTaskToUserCommandRequest, AssociateTaskToUserCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public AssociateTaskToUserCommandHandler(ITaskRepository taskRepository, INotificationService notificationService, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
        }

        public async Task<AssociateTaskToUserCommandResponse> Handle(AssociateTaskToUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new AssociateTaskToUserCommandResponse()
                    {
                        Message = $"Usuário com ID {request.UserId} não encontrado.",
                        Success = false,
                        ErrorCode = ErrorCodes.USER_NOT_FOUND
                    };
                }
                var task = await _taskRepository.GetByIdAsync(request.TaskId);
                if (task == null)
                {
                    return new AssociateTaskToUserCommandResponse()
                    {
                        Message = $"Tarefa com ID {request.TaskId} não encontrada.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                }
                task.Associate(user);
                await _taskRepository.UpdateAsync(task);

                await _notificationService.NotifyUserAsync(task.User.Id, $"Nova tarefa atribuída: {task.Title}");

                var response = new AssociateTaskToUserCommandResponse()
                {
                    Data = TaskDto.MapToDto(task),
                    Message = "Tarefa associada com sucesso.",
                    Success = true,
                };
                return response;
            }
            catch (TaskAlreadyAssociatedException ex)
            {
                return new AssociateTaskToUserCommandResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ErrorCode = ErrorCodes.TASK_ALREADY_ASSOCIATED
                };
            }
            catch (WrongRequiredInformation ex)
            {
                return new AssociateTaskToUserCommandResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION
                };
            }
            catch (Exception ex)
            {
                return new AssociateTaskToUserCommandResponse()
                {
                    Message = $"Ocorreu um erro inesperado: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
