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

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
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

                var userName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _userRepository.GetByUsernameAsync(userName);
                if(user == null)
                {
                    return new CreateTaskCommandResponse
                    {
                        Message = "User not found.",
                        Success = false,
                        ErrorCode = ErrorCodes.USER_NOT_FOUND
                    };
                }
                var task = new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = request.Data.Title,
                    Description = request.Data.Description,
                    DueDate = request.Data.DueDate,
                    User = user
                };

                var created = await _taskRepository.CreateAsync(task);

                var response = new CreateTaskCommandResponse()
                {
                    Message = "Task created successfully.",
                    Success = true,
                    Data = TaskDto.MapToDto(created)
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
