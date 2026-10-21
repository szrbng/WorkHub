using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Results;

namespace WorkHub.Application.Features.Auth.LogoutAll
{
    public sealed record LogoutAllCommand : IRequest<Result>;
}
