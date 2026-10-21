using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
