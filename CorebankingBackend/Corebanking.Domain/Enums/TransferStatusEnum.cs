using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Enums
{
    public enum TransferStatusEnum
    {
        Pending = 1,
        Processing = 2,
        Successful = 3,
        Failed = 4,
        Reversed = 5,
        Cancelled = 6
    }
}
