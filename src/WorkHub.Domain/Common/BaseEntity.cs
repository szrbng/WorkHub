using System;
using System.Collections.Generic;
using System.Text;

namespace WorkHub.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    }
}
