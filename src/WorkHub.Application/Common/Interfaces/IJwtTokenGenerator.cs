using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Domain.Entities;

namespace WorkHub.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiresAtUtc) Generate(User user);
    }
}
