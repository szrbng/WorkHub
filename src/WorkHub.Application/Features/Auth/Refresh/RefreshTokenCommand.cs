using FluentValidation;
using MediatR;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Refresh
{
    public sealed record RefreshTokenCommand(string RefreshToken)
        : IRequest<Result<AuthResponse>>;

    public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
