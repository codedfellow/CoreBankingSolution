using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Enums
{
    public enum TransactionTypeEnum
    {
        Debit = 1,
        Credit = 2,
        TransferOut = 3,
        TransferIn = 4,
        Fee = 5,
        Reversal = 6,
    }
}
