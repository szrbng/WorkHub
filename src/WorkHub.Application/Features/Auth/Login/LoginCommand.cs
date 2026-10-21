using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Login
{
    public sealed record LoginCommand(string Email, string Password)
        : IRequest<Result<AuthResponse>>;
}
