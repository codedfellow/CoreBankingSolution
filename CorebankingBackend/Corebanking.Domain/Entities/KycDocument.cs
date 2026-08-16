using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class KycDocument : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public KycDocumentTypeEnum DocumentType { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime UploadedAtUtc { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public VerificationStatusEnum VerificationStatus { get; set; }
        public Guid? ReviewedBy { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public string? RejectionReason { get; set; }
        public CustomerProfile? CustomerProfile { get; set; } = null;
    }
}
