using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Contracts.Configurations
{
    public interface IPersistenceConfigurations
    {
        public string ConnectionString { get; }
    }
}
