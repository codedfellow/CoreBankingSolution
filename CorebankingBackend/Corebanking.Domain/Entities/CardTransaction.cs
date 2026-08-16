using Corebanking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class CardTransaction : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid TransactionId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantCategory { get; set; }
        public string TerminalId { get; set; }
        public string Location { get; set; }
        public string AuthCode { get; set; }
        public bool IsInternational { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Transaction? Transaction { get; set; } = null;
        public Card? Card { get; set; } = null;
    }
}
