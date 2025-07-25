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

namespace TaskManagerApplication.Tasks.Commands.Tasks.DeleteTask
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
                        Message = "Task not found.",
                        Success = false,
                        ErrorCode = ErrorCodes.TASK_NOT_FOUND
                    };
                }

                existingTask.CanDelete();

                await _taskRepository.DeleteAsync(existingTask.Id);

                var response = new DeleteTaskCommandResponse()
                {
                    Message = "Task deleted successfully.",
                    Success = true
                };

                return response;
            }
            catch (TaskWithAssociatedUserCannotBeDeleted ex)
            {
                return new DeleteTaskCommandResponse
                {
                    Message = ex.Message,
                    Success = false,
                    ErrorCode = ErrorCodes.TASK_ASSOCIATED_CAN_NOT_BE_DELETED
                };
            }
            catch (Exception ex)
            {
                return new DeleteTaskCommandResponse
                {
                    Message = $"An error occurred while creating the task: {ex.Message}",
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR
                };
            }
        }
    }
}
