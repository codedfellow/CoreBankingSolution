using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class Transfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToBankCode { get; set; }
        public string ToBankName { get; set; }
        public string ToAccountName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Narration { get; set; }
        public TransferTypeEnum TransferType { get; set; }
        public TransferStatusEnum Status { get; set; }
        public string FailureReason { get; set; }
        public string IdempotencyKey { get; set; }
        public Guid InitiatedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public bool RequiresApproval { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public DateTime? ProcessedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Account? FromAccount { get; set; } = null;
        public Account? ToAccount { get; set; } = null;
    }
}
