using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private static readonly Error InvalidCredentials =
            Error.Unauthorized("Auth.InvalidCredentials", "E-posta veya şifre hatalı.");

        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null ||
                !user.IsActive ||
                !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Failure(InvalidCredentials);
            }

            var (token, expiresAtUtc) = _jwtTokenGenerator.Generate(user);

            return Result<LoginResponse>.Success(new LoginResponse(token, expiresAtUtc));
        }
    }
}
