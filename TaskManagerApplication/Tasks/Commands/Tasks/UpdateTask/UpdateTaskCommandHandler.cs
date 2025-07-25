using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Commands.Tasks.CreateTask;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplication.Tasks.Commands.Tasks.UpdateTask
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
                if (request.Task.Id == Guid.Empty)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Id is required.",
                        Success = false,
                        ErrorCode = ErrorCodes.MISSING_INFORMATION
                    };
                }
                var existingTask = await _taskRepository.GetByIdAsync(request.Task.Id);
                if (existingTask == null)
                {
                    return new UpdateTaskCommandResponse
                    {
                        Message = "Task not found.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                }

                existingTask.DueDate = request.Task.DueDate;
                existingTask.Status = request.Task.Status;
                existingTask.Title = request.Task.Title;
                existingTask.Description = request.Task.Description;

                if (request.Task.IdUser != null && request.Task.IdUser != Guid.Empty && request.Task.IdUser != existingTask.User?.Id)
                {
                    existingTask.User = await _userRepository.GetByIdAsync(request.Task.IdUser);
                }

                existingTask.IsValid();
                
                var updatedTask = await _taskRepository.UpdateAsync(existingTask);

                var response = new UpdateTaskCommandResponse()
                {
                    Message = "Task updated successfully.",
                    Success = true,
                    Task = TaskDto.MapToDto(updatedTask)
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
