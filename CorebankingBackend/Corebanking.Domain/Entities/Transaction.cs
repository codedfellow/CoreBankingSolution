using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class Transaction : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string ExternalReference { get; set; }
        public string Narration { get; set; }
        public TransactionChannelEnum Channel { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public DateOnly ValueDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid? ProcessedBy { get; set; }
        public Guid? ReversalOf { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public CardTransaction? CardTransaction { get; set; } = null;
        public ICollection<Transaction> Reversals { get; set; } = [];
        public Transaction? ReversalTransaction { get; set; } = null;
        public Account? Account { get; set; } = null;
        public ICollection<TransactionFee> TransactionFees { get; set; } = [];
    }
}
