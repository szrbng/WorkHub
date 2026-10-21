using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Results;
using WorkHub.Domain.Enums;

namespace WorkHub.Application.Features.Users.ChangeRole
{
    public sealed record ChangeUserRoleCommand(Guid UserId, UserRole Role) : IRequest<Result>;

    public sealed class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
    {
        public ChangeUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Role).IsInEnum();
        }
    }
}
