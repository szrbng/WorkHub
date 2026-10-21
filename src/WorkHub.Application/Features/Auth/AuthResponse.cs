using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Application.Features.Auth
{
    public sealed record AuthResponse(
        string AccessToken,
        DateTime AccessTokenExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc);
}
