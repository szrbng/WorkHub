using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private static readonly Error InvalidCredentials =
            Error.Unauthorized("Auth.InvalidCredentials", "E-posta veya şifre hatalı.");

        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthResponse>> Handle(
            LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null ||
                !user.IsActive ||
                !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result<AuthResponse>.Failure(InvalidCredentials);
            }

            var (accessToken, accessExpiresAtUtc) = _jwtTokenGenerator.Generate(user);

            var refresh = _refreshTokenGenerator.Generate();
            user.AddRefreshToken(refresh.Hash, refresh.ExpiresAtUtc);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<AuthResponse>.Success(new AuthResponse(
                accessToken, accessExpiresAtUtc, refresh.Token, refresh.ExpiresAtUtc));
        }
    }
}
