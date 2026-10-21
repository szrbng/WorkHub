using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Application.Common.Results;
using WorkHub.Domain.Entities;
using WorkHub.Domain.Enums;

namespace WorkHub.Application.Features.Auth.Register
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RegisterResponse>> Handle(
            RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _users.EmailExistsAsync(request.Email, cancellationToken))
            {
                return Result<RegisterResponse>.Failure(
                    Error.Conflict("Auth.EmailTaken", "Bu e-posta adresi zaten kayıtlı."));
            }

            var hash = _passwordHasher.Hash(request.Password);

            var user = User.Create(request.Email, request.FullName, hash, UserRole.Employee);

            await _users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<RegisterResponse>.Success(new RegisterResponse(user.Id, user.Email));
        }
    }
}
