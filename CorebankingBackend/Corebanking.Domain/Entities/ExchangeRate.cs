using Corebanking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class ExchangeRate : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal MidRate { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
