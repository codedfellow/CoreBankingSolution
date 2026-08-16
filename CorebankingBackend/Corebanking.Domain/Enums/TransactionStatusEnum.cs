using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Enums
{
    public enum TransactionStatusEnum
    {
        Pending = 1,
        Successful = 2,
        Failed = 3,
        Reversed = 4,
    }
}
