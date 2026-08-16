using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class ScheduledTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FromAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToBankCode { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public TransferFrequencyEnum Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly NextRunDate { get; set; }
        public DateOnly LastRunDate { get; set; }
        public ScheduledTransferStatusEnum Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public CustomerProfile? CustomerProfile { get; set; } = null;
        public Account FromAccount { get; set; } = null;
    }
}
