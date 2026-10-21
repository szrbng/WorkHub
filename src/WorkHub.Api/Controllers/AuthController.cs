using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkHub.Api.Extensions;
using WorkHub.Application.Features.Auth.Login;
using WorkHub.Application.Features.Auth.Register;

namespace WorkHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender) => _sender = sender;

        [HttpPost("register")]
        public async Task<IActionResult> Register(
    RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? StatusCode(StatusCodes.Status201Created, result.Value)
                : result.ToProblem();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [Authorize]
        [HttpGet("me")] //token test etmek için
        public IActionResult Me() => Ok(new
        {
            id = User.FindFirst("sub")?.Value,
            email = User.FindFirst("email")?.Value,
            name = User.FindFirst("name")?.Value,
            role = User.FindFirst("role")?.Value
        });
    }
}
