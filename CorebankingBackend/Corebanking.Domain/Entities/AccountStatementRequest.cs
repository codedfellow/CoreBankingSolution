using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class AccountStatementRequest : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid RequestedBy { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public StatementFormatEnum Format { get; set; }
        public StatementStatusEnum StatementStatusEnum { get; set; }
        public string FileUrl { get; set; }
        public DateTime RequestedAtUtc { get; set; }
        public DateTime? GeneratedAtUtc { get; set; }
        public Account? Account { get; set; } = null;
    }
}
