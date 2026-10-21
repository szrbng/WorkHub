using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Users.ChangeRole
{
    public sealed class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeUserRoleCommandHandler(
            ICurrentUser currentUser, IUserRepository users, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _users = users;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == _currentUser.UserId)
            {
                return Result.Failure(Error.Forbidden(
                    "Users.CannotChangeOwnRole", "Kendi rolünü değiştiremezsin."));
            }

            var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Result.Failure(Error.NotFound("Users.NotFound", "Kullanıcı bulunamadı."));

            user.ChangeRole(request.Role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
