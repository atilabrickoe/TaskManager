﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication;
using TaskManagerApplication.Tasks.Commands.Tasks.AssociateTaskToUser;
using TaskManagerApplication.Tasks.Commands.Tasks.CreateTask;
using TaskManagerApplication.Tasks.Commands.Tasks.DeleteTask;
using TaskManagerApplication.Tasks.Commands.Tasks.UpdateTask;
using TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers;
using TaskManagerApplication.Tasks.Queries.Tasks;
using TaskManagerApplication.Tasks.Queries.Users.GetAllUsers;
using TaskManagerApplication.Tasks.Queries.Users.GetById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<TaskController> _logger;

        public TaskController(IMediator mediatR, ILogger<TaskController> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }
        [HttpPost("CreateTask")]
        public async Task<ActionResult<CreateTaskCommandResponse>> CreateTask(CreateTaskCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res.Task);
            else
            {
                _logger.LogError("An unexpected error occurred: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpPost("AssociateTaskToUser")]
        public async Task<ActionResult<AssociateTaskToUserCommandResponse>> AssociateTaskToUser(AssociateTaskToUserComandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res.Task);
            else
            {
                _logger.LogError("An unexpected error occurred: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpGet("GetAllTasks/{withUser}")]
        public async Task<ActionResult<GetAllTasksQueryResponse>> GetAllTasks(bool withUser = false)
        {
            var request = new GetAllTasksQueryRequest()
            {
                WithUser = withUser
            };
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res.Task);
            else
            {
                _logger.LogError(res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpPost("UpdateTask")]
        public async Task<ActionResult<UpdateTaskCommandResponse>> UpdateTask(UpdateTaskCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res.Task);
            else
            {
                _logger.LogError("An unexpected error occurred: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<ActionResult<DeleteTaskCommandResponse>> UpdateTask(Guid taskId)
        {
            var request = new DeleteTaskCommandRequest()
            {
                TaskId = taskId
            };
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res.Message);
            else
            {
                _logger.LogError("An unexpected error occurred: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
    }
}
