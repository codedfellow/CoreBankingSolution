using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class AccountStatementRequestConfiguration : IEntityTypeConfiguration<AccountStatementRequest>
    {
        public void Configure(EntityTypeBuilder<AccountStatementRequest> builder)
        {
            builder.HasOne<AppIdentityUser>()
                   .WithMany()
                   .HasForeignKey(x => x.RequestedBy)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Account)
                   .WithMany(x => x.AccountStatementRequests)
                   .HasForeignKey(x => x.AccountId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
