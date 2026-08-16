using Corebanking.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Data
{
    public class BankingDbContextFactory
    : IDesignTimeDbContextFactory<BankingDbContext>
    {
        public BankingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankingDbContext>();

            optionsBuilder.UseNpgsql(
                    "type connection string here and revert after adding migrations",
                    npgsql =>
                    {
                        npgsql.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay:
                                TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });

            return new BankingDbContext(optionsBuilder.Options);
        }
    }
}
