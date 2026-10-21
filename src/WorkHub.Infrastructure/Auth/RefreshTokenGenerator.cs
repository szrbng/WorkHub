using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using WorkHub.Application.Common.Interfaces;

namespace WorkHub.Infrastructure.Auth
{
    public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly JwtSettings _settings;

        public RefreshTokenGenerator(IOptions<JwtSettings> options)
            => _settings = options.Value;

        public GeneratedRefreshToken Generate()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes);

            return new GeneratedRefreshToken(
                token,
                Hash(token),
                DateTime.UtcNow.AddDays(_settings.RefreshTokenDays));
        }

        public string Hash(string token)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
