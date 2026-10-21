using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkHub.Api.Extensions;
using WorkHub.Application.Features.Auth.Login;
using WorkHub.Application.Features.Auth.Register;
using WorkHub.Application.Features.Auth.LogoutAll;
using WorkHub.Application.Features.Auth.Logout;
using WorkHub.Application.Features.Auth.Refresh;

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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
    RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            LogoutCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new LogoutAllCommand(), cancellationToken);

            return result.IsSuccess
                ? NoContent()
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
