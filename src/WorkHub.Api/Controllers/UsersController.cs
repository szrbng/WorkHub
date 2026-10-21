using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkHub.Api.Extensions;
using WorkHub.Application.Features.Users.ChangeRole;
using WorkHub.Domain.Enums;

namespace WorkHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender) => _sender = sender;

        [HttpPut("{id:guid}/role")]
        public async Task<IActionResult> ChangeRole(
    Guid id, ChangeUserRoleRequest body, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ChangeUserRoleCommand(id, body.Role), cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
    public sealed record ChangeUserRoleRequest(UserRole Role);
}
