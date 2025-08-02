using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication.Tasks.Commands.AssociateTaskToUser;
using TaskManagerApplication.Tasks.Commands.CreateTask;
using TaskManagerApplication.Tasks.Commands.DeleteTask;
using TaskManagerApplication.Tasks.Commands.UpdateTask;
using TaskManagerApplication.Tasks.Queries.GetAllTasks;
using TaskManagerApplication.Tasks.Queries.GetTaskById;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
                return Created("", res);
            else
            {
                _logger.LogError("Ocorreu um erro inesperado: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpPost("AssociateTaskToUser")]
        public async Task<ActionResult<AssociateTaskToUserCommandResponse>> AssociateTaskToUser(AssociateTaskToUserCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res);
            else
            {
                _logger.LogError("Ocorreu um erro inesperado: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
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
                return Ok(res);
            else
            {
                _logger.LogError(res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpGet("GetTaskById/{id}/{withUser}")]
        public async Task<ActionResult<GetAllTasksQueryResponse>> GetTaskById(Guid id,bool withUser = false)
        {
            var request = new GetTaskByIdQueryRequest()
            {
                Id = id,
                WithUser = withUser
            };
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res);
            else
            {
                _logger.LogError(res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpPost("UpdateTask")]
        public async Task<ActionResult<UpdateTaskCommandResponse>> UpdateTask(UpdateTaskCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res);
            else
            {
                _logger.LogError("Ocorreu um erro inesperado: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<ActionResult<DeleteTaskCommandResponse>> DeleteTask(Guid taskId)
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
                _logger.LogError("Ocorreu um erro inesperado: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
    }
}
