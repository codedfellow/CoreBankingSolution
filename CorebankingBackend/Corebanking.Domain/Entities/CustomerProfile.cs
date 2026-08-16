using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public class CustomerProfile : AuditableEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string CustomerNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public IdentificationTypeEnum NationalIdType { get; set; }
        public string NationalIdNumber { get; set; }
        public string BvnNumber { get; set; }
        public KycStatusEnum KycStatus { get; set; }
        public DateTime? KycVerifiedAtUtc { get; set; }
        public Guid? KycVerifiedBy { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public ICollection<Account> Accounts { get; set; } = [];
        public ICollection<Beneficiary> Beneficiaries{ get; set; } = [];
        public ICollection<CustomerAddress> CustomerAddresses { get; set; } = [];
        public ICollection<KycDocument> KycDocuments { get; set; } = [];
        public ICollection<NextOfKin> NextOfKins { get; set; } = [];
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<ScheduledTransfer> ScheduledTransfers { get; set; } = [];
    }
}
