using MediatR;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommandRequest, UpdateTaskCommandResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data.Id == Guid.Empty)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Id is required.",
                        Success = false,
                        ErrorCode = ErrorCodes.MISSING_INFORMATION
                    };
                }
                var existingTask = await _taskRepository.GetByIdAsync(request.Data.Id);
                if (existingTask == null)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Task not found.",
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
                    Message = "Task updated successfully.",
                    Success = true,
                    Data = TaskDto.MapToDto(updatedTask)
                };

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
                    Message = $"An error occurred while updating the task: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
