using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;

namespace WorkHub.Infrastructure.Auth
{
    public sealed class BCryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12;

        public string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

        public bool Verify(string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
