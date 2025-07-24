using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApplication;
using TaskManagerApplication.Tasks.Commands.CreateRandomUsers;
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
        [HttpPost("CreateRandomUsers")]
        public async Task<ActionResult<CreateRandomUsersCommandResponse>> CreateRandomUsers(CreateRandomUsersCommandRequest request)
        {
            var res = await _mediatR.Send(request);
            if (res.Success)
                return Created("", res.Users);
            if (res.ErrorCode == ErrorCodes.MISSING_INFORMATION)
            {
                _logger.LogError("Erro ao criar Usuarios data: {Message}", res.Message);
                return StatusCode(res.ErrorCode.GetHashCode(), res.Message);
            }
            else
            {
                _logger.LogError("An unexpected error occurred: {Message}", res.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
