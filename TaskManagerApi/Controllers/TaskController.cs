using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication;
using TaskManagerApplication.Tasks.Commands.Tasks.AssociateTaskToUser;
using TaskManagerApplication.Tasks.Commands.Tasks.CreateTask;
using TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers;
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
        public async Task<ActionResult<CreateTaskCommandResponse>> CreateTask(CreateTaskComandRequest request)
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
    }
}
