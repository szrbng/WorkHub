using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Register
{
    public sealed record RegisterCommand(
        string Email,
        string FullName,
        string Password) : IRequest<Result<RegisterResponse>>;

    public sealed record RegisterResponse(Guid UserId, string Email);
}
