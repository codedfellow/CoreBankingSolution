using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class AuditLog : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public TransactionChannelEnum Channel { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
