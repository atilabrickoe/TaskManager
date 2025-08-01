using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication.Users.Commands.CreateRandomUsers;
using TaskManagerApplication.Users.Commands.CreateUser;
using TaskManagerApplication.Users.Commands.Login;
using TaskManagerApplication.Users.Queries.GetAllUsers;
using TaskManagerApplication.Users.Queries.GetById;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediatR, ILogger<UserController> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult<CreateUserCommandResponse>> CreateUser(CreateUserCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res);
            else
            {
                _logger.LogError("Erro ao criar Usuarios data: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginCommandResponse>> Login(LoginCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Ok(res);
            else
            {
                _logger.LogError("Erro ao criar Usuarios data: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpPost("CreateRandom")]
        public async Task<ActionResult<CreateRandomUsersCommandResponse>> CreateRandomUsers(CreateRandomUsersCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res);
            else
            {
                _logger.LogError("Erro ao criar Usuarios data: {Message}", res.Message);
                return StatusCode(res.ErrorCodeHttp, res);
            }
        }
        [HttpGet("GetAllUsers/{withTask}")]
        public async Task<ActionResult<GetAllUsersQueryResponse>> GetAllUsers(bool withTask = false)
        {
            var request = new GetAllUsersQueryRequest()
            {
                WithTask = withTask
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
        [HttpGet("GetUserById/{id}/{withTask}")]
        public async Task<ActionResult<GetAllUsersQueryResponse>> GetUserById(Guid id, bool withTask = false)
        {
            var request = new GetUserByIdQueryRequest()
            {
                WithTask = withTask,
                Id = id
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
        
    }
}
