using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class Account : AuditableEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal LedgerBalance { get; set; }
        public string Currency { get; set; }
        public AccountStatusEnum Status { get; set; }
        public TierLevelEnum TierLevel { get; set; }
        public decimal DailyTransactionLimit { get; set; }
        public decimal SingleTransactionLimit { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime? MaturityDateUtc { get; set; }
        public DateTime OpenedAtUtc { get; set; }
        public DateTime? ClosedAtUtc { get; set; }
        public CustomerProfile? CustomerProfile { get; set; }
        public ICollection<AccountMandate> AccountMandates { get; set; } = [];
        public ICollection<AccountStatementRequest> AccountStatementRequests { get; set; } = [];
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<ScheduledTransfer> ScheduledTransfers { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];
        public ICollection<Transfer> TransfersOut { get; set; } = [];
        public ICollection<Transfer> TransfersIn { get; set; } = [];
    }
}
