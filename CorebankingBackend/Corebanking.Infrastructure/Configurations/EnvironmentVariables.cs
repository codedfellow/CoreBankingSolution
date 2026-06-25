using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Infrastructure.Configurations
{
    public static class EnvironmentVariables
    {
        public static string ConnectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? throw new ArgumentNullException("Connection string not provided");
    }
}
