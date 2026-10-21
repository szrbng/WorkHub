using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
    }
}
