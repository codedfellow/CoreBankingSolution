using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Domain.Common
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
