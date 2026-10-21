using Microsoft.AspNetCore.Mvc;
using WorkHub.Application.Common.Results;

namespace WorkHub.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToProblem(this Result result)
        {
            var error = result.Error
                ?? throw new InvalidOperationException("Başarılı bir Result problem'e çevrilemez.");

            var status = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Status = status,
                Title = error.Code,
                Detail = error.Message
            };

            return new ObjectResult(problem) { StatusCode = status };
        }
    }
}
