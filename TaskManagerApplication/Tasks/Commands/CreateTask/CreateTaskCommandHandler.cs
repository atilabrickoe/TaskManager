using MediatR;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskComandRequest, CreateTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, INotificationService notificationService, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
        }

        public async Task<CreateTaskCommandResponse> Handle(CreateTaskComandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.LoggedUserId);

            if(user == null)
                return new CreateTaskCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.USER_NOT_FOUND,
                    Message = "Usuário não encontrado."
                };

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Data.Title,
                Description = request.Data.Description,
                DueDate = request.Data.DueDate,
                User = user,
            };

            var created = await _taskRepository.CreateAsync(task);
            await _notificationService.NotifyUserAsync(task.User.Id, $"Nova tarefa atribuída: {task.Title}");

            var response = new CreateTaskCommandResponse()
            {
                Message = "Tarefa criada com sucesso.",
                Success = true,
            };

            return response;
        }
    }
}
