using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class AccountMandate : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public MandateTypeEnum MandateType { get; set; }
        public bool CanDebit { get; set; }
        public bool CanCredit { get; set; }
        public decimal MaxTransactionAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
        public Account? Account { get; set; } = null;
    }
}
