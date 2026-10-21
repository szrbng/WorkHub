using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Refresh
{
    public sealed class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private static readonly Error InvalidToken =
            Error.Unauthorized("Auth.InvalidRefreshToken", "Refresh token geçersiz.");

        private readonly IUserRepository _users;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IUserRepository users,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _users = users;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthResponse>> Handle(
            RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var hash = _refreshTokenGenerator.Hash(request.RefreshToken);

            var user = await _users.GetByRefreshTokenHashAsync(hash, cancellationToken);
            var existing = user?.FindRefreshToken(hash);

            if (user is null || existing is null)
                return Result<AuthResponse>.Failure(InvalidToken);

            if (!existing.IsActive)
            {
                // İptal edilmiş token tekrar kullanıldı: çalınma şüphesi.
                if (existing.RevokedAtUtc is not null)
                {
                    user.RevokeAllRefreshTokens();
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return Result<AuthResponse>.Failure(InvalidToken);
            }

            if (!user.IsActive)
                return Result<AuthResponse>.Failure(InvalidToken);

            // Rotation: eskiyi iptal et, yenisini ver.
            existing.Revoke();

            var newRefresh = _refreshTokenGenerator.Generate();
            user.AddRefreshToken(newRefresh.Hash, newRefresh.ExpiresAtUtc);

            var (accessToken, accessExpiresAtUtc) = _jwtTokenGenerator.Generate(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<AuthResponse>.Success(new AuthResponse(
                accessToken, accessExpiresAtUtc, newRefresh.Token, newRefresh.ExpiresAtUtc));
        }
    }
}
