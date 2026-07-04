using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Infrastructure.Configurations
{
    internal static class EnvironmentVariables
    {
        public static string ConnectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? throw new ArgumentNullException("Connection string not provided");
        public static int RefreshTokenDays = int.Parse(Environment.GetEnvironmentVariable("RefreshTokenDays") ?? "4");
        public static string JwtIssuer = Environment.GetEnvironmentVariable("JwtIssuer") ?? throw new ArgumentNullException("Jwt Issuer not set");
        public static string JwtAudience = Environment.GetEnvironmentVariable("JwtAudience") ?? throw new ArgumentNullException("Jwt Audience not set");
        public static string JwtSigningKey = Environment.GetEnvironmentVariable("JwtSigningKey") ?? throw new ArgumentNullException("Jwt signing key not set");
        public static int AccessTokenMinutes = int.Parse(Environment.GetEnvironmentVariable("AccessTokenMinutes") ?? "4");
    }
}
