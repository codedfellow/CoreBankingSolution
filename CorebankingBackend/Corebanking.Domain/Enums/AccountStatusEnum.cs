using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Enums
{
    public enum AccountStatusEnum
    {
        Active = 1,
        Dormant = 2,
        Frozen = 3,
        Closed = 4,
        PendingActivation = 5
    }
}
