using Corebanking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class Beneficiary : AuditableEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Nickname { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFavourite { get; set; }
        public DateTime? LastUsedAtUtc { get; set; }
        public CustomerProfile? CustomerProfile { get; set; }
    }
}
