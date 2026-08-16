using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class TransactionFee : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public FeeTypeEnum FeeType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Transaction? Transaction { get; set; } = null;
    }
}
