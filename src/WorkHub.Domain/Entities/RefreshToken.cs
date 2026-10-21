using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Domain.Common;

namespace WorkHub.Domain.Entities
{
    public class RefreshToken :BaseEntity
    {
        private RefreshToken() { } // EF Core için

        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
        public bool IsActive => RevokedAtUtc is null && !IsExpired;

        internal static RefreshToken Create(Guid userId, string tokenHash, DateTime expiresAtUtc)
        {
            return new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAtUtc = expiresAtUtc
            };
        }

        public void Revoke() => RevokedAtUtc ??= DateTime.UtcNow;

    }
}
