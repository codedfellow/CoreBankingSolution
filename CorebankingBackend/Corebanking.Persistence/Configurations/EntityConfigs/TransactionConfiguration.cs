using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne<AppIdentityUser>()
               .WithMany()
               .HasForeignKey(x => x.ProcessedBy)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReversalTransaction)
                   .WithMany(x => x.Reversals)
                   .HasForeignKey(x => x.ReversalOf)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Account)
                   .WithMany(x => x.Transactions)
                   .HasForeignKey(x => x.AccountId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
