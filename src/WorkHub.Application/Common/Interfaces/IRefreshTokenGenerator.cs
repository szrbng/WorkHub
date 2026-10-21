using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Application.Common.Interfaces
{
    public sealed record GeneratedRefreshToken(string Token, string Hash, DateTime ExpiresAtUtc);

    public interface IRefreshTokenGenerator
    {
        GeneratedRefreshToken Generate();
        string Hash(string token);
    }

    //Token istemciye gider, Hash veritabanına yazılır. Düz token’ı asla saklamıyoruz.
}
