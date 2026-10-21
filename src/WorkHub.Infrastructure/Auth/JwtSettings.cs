using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Infrastructure.Auth
{
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Secret { get; set; } = default!;
        public int AccessTokenMinutes { get; set; } = 15;
        public int RefreshTokenDays { get; set; } = 7;
    }
}
