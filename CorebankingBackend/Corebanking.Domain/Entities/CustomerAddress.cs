using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class CustomerAddress : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public AddressTypeEnum AddressType { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public CustomerProfile? CustomerProfile { get; set; } = null;
    }
}
