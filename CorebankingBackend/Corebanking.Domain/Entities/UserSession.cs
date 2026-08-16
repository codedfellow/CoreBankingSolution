using Corebanking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Entities
{
    public sealed class UserSession : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime LastActivityAtUtc { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
        public Guid RefreshTokenId { get; set; }
    }
}
