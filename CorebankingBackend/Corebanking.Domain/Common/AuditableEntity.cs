using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Common
{
    public abstract class AuditableEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; protected set; } = default!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
