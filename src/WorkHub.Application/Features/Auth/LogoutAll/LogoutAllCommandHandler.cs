using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.LogoutAll
{
    public sealed class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutAllCommandHandler(
            ICurrentUser currentUser, IUserRepository users, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _users = users;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is not { } userId)
                return Result.Failure(Error.Unauthorized("Auth.Unauthenticated", "Oturum bulunamadı."));

            var user = await _users.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return Result.Failure(Error.NotFound("Users.NotFound", "Kullanıcı bulunamadı."));

            user.RevokeAllRefreshTokens();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
