using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.Tasks.CreateTask
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
            if (string.IsNullOrWhiteSpace(request.Data.Title))
            {
                return new CreateTaskCommandResponse
                {
                    Message = "Title is required.",
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION
                };
            }
            var existingTask = await _taskRepository.GetByTitleAsync(request.Data.Title);
            if(existingTask != null)
            {
                return new CreateTaskCommandResponse
                {
                    Message = "A task with this title already exists.",
                    Success = false,
                    ErrorCode = ErrorCodes.TASK_TITLE_ALREADY_EXISTS
                };
            }
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Data.Title,
                Description = request.Data.Description,
                DueDate = request.Data.DueDate
            };

            var created = await _taskRepository.CreateAsync(task);

            var response = new CreateTaskCommandResponse()
            {
                Message = "Task created successfully.",
                Success = true,
                Task = TaskDto.MapToDto(created)
            };

            return response;
        }
    }
}
