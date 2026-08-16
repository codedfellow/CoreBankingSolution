using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class TransactionLimit : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public TierLevelEnum TierLevel { get; set; }
        public TransactionChannelEnum Channel { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal SingleLimit { get; set; }
        public int DailyCount { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
