using Corebanking.Domain.Common;
using Corebanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class Card : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string CardNumberHash { get; set; }
        public CardTypeEnum CardType { get; set; }
        public CardSchemeEnum Scheme { get; set; }
        public string NameOnCard { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public CardStatusEnum Status { get; set; }
        public bool IsVirtual { get; set; }
        public decimal DailyLimit { get; set; }
        public bool IsContactless { get; set; }
        public bool IsPosEnabled { get; set; }
        public bool IsOnlineEnabled { get; set; }
        public bool IsInternationalEnabled { get; set; }
        public DateTime IssuedAtUtc { get; set; }
        public DateTime? BlockedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Account? Account { get; set; } = null;
        public CustomerProfile? CustomerProfile { get; set; } = null;
        public ICollection<CardTransaction> CardTransactions { get; set; } = [];
    }
}