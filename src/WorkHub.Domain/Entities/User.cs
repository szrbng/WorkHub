using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Domain.Common;
using WorkHub.Domain.Enums;

namespace WorkHub.Domain.Entities
{
    public class User : BaseEntity
    {
        private readonly List<RefreshToken> _refreshTokens = new();

        private User() { } // EF Core için

        public string Email { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public UserRole Role { get; private set; }
        public bool IsActive { get; private set; } = true;

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public static User Create(string email, string fullName, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş olamaz.", nameof(email));
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Ad soyad boş olamaz.", nameof(fullName));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Şifre hash'i boş olamaz.", nameof(passwordHash));

            return new User
            {
                Email = email.Trim().ToLowerInvariant(),
                FullName = fullName.Trim(),
                PasswordHash = passwordHash,
                Role = role
            };
        }

        public void AddRefreshToken(string tokenHash, DateTime expiresAtUtc)
        {
            _refreshTokens.Add(RefreshToken.Create(Id, tokenHash, expiresAtUtc));
        }

        public void Deactivate() => IsActive = false;

        public RefreshToken? FindRefreshToken(string tokenHash)
    => _refreshTokens.FirstOrDefault(t => t.TokenHash == tokenHash);

        public void RevokeAllRefreshTokens()
        {
            foreach (var token in _refreshTokens.Where(t => t.IsActive))
                token.Revoke();
        }

        public void ChangeRole(UserRole newRole)
        {
            if (!Enum.IsDefined(newRole))
                throw new ArgumentOutOfRangeException(nameof(newRole), "Geçersiz rol.");

            Role = newRole;
        }
    }
}
