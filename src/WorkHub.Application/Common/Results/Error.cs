using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Application.Common.Results
{
    public enum ErrorType
    {
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden
    }
    public sealed record Error(string Code, string Message, ErrorType Type)
    {
        public static Error Conflict(string code, string message)
            => new(code, message, ErrorType.Conflict);

        public static Error Unauthorized(string code, string message)
            => new(code, message, ErrorType.Unauthorized);

        public static Error NotFound(string code, string message)
            => new(code, message, ErrorType.NotFound);
    }
}
