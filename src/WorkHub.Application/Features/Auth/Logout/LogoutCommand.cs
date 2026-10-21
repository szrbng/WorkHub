using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest<Result>;

    public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
