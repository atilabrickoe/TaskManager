using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.Tasks.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommandRequest, CreateTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommandRequest request, CancellationToken cancellationToken)
        {
            try
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
                if (existingTask != null)
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
                    Message = $"An error occurred while creating the task: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
