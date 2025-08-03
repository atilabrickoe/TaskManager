using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommandRequest, CreateTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, INotificationService notificationService)
        {
            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Data.Title))
                {
                    return new CreateTaskCommandResponse
                    {
                        Message = "Título é obrigatório.",
                        Success = false,
                        ErrorCode = ErrorCodes.MISSING_INFORMATION
                    };
                }

                var userName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _userRepository.GetByUsernameAsync(userName);
                if(user == null)
                {
                    return new CreateTaskCommandResponse
                    {
                        Message = "Título é obrigatório.",
                        Success = false,
                        ErrorCode = ErrorCodes.USER_NOT_FOUND
                    };
                }
                var existingTask = await _taskRepository.TaskExistsToUser(request.Data.Title, user.Id);
                if (existingTask)
                {
                    return new CreateTaskCommandResponse
                    {
                        Message = "Já existe uma tarefa com este título para este usuário.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_TITLE_ALREADY_EXISTS
                    };
                }
                var task = new TaskItem
                {
                    Title = request.Data.Title,
                    Description = request.Data.Description,
                    DueDate = request.Data.DueDate,
                    Status = request.Data.Status,
                    User = user
                };

                var created = await _taskRepository.CreateAsync(task);

                var response = new CreateTaskCommandResponse()
                {
                    Message = "Tarefa criada com sucesso.",
                    Success = true,
                    Data = TaskDto.MapToDto(created)
                };
                await _notificationService.NotifyUserAsync(task.User.Id, $"Nova tarefa atribuída ao usuário {task.User.UserName}, tarefa: {task.Title}, data de vencimento: {task.DueDate.ToString("dd/MM/yyyy")}");

                return response;
            }
            catch (WrongRequiredInformation ex)
            {
                return new CreateTaskCommandResponse
                {
                    Message = ex.Message,
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION
                };
            }
            catch (Exception ex)
            {
                return new CreateTaskCommandResponse
                {
                    Message = $"Ocorreu um erro ao criar a tarefa: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
