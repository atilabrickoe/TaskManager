using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication;
using TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers;
using TaskManagerApplication.Tasks.Queries.Users.GetAllUsers;
using TaskManagerApplication.Tasks.Queries.Users.GetById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediatR, ILogger<UserController> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }
        [HttpPost("CreateRandom")]
        public async Task<ActionResult<CreateRandomUsersCommandResponse>> CreateRandomUsers(CreateRandomUsersCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res.Users);
            else
            {
                _logger.LogError("Erro ao criar Usuarios data: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpGet("GetAllUsers/{withTask}")]
        public async Task<ActionResult<GetAllUsersQueryResponse>> ObterUsuarios(bool withTask = false)
        {
            var request = new GetAllUsersQueryRequest()
            {
                WithTask = withTask
            };
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res.Users);
            else
            {
                _logger.LogError(res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
        [HttpGet("GetAllUsersById/{id}/{withTask}")]
        public async Task<ActionResult<GetAllUsersQueryResponse>> GetAllUsersById(Guid id, bool withTask = false)
        {
            var request = new GetUserByIdQueryRequest()
            {
                WithTask = withTask,
                Id = id
            };
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res.User);
            else
            {
                _logger.LogError(res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
        }
    }
}
