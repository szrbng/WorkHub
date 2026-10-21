using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.Logout
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IUserRepository _users;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(
            IUserRepository users,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _users = users;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var hash = _refreshTokenGenerator.Hash(request.RefreshToken);
            var user = await _users.GetByRefreshTokenHashAsync(hash, cancellationToken);

            if (user is not null)
            {
                user.FindRefreshToken(hash)?.Revoke();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // Token bilinmese bile başarılı dön: logout idempotent olmalı ve bilgi sızdırmamalı.
            return Result.Success();
        }
    }
}
