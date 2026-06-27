using Corebanking.Application.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations
{
    public class PersistenceConfigurations : IPersistenceConfigurations
    {
        public string ConnectionString { get; set; } = EnvironmentVariables.ConnectionString;
    }
}
