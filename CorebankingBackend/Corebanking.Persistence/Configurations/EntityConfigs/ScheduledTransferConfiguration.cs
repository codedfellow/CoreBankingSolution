using Corebanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class ScheduledTransferConfiguration : IEntityTypeConfiguration<ScheduledTransfer>
    {
        public void Configure(EntityTypeBuilder<ScheduledTransfer> builder)
        {
            builder.HasOne(x => x.CustomerProfile)
                   .WithMany(x => x.ScheduledTransfers)
                   .HasForeignKey(x => x.CustomerId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.FromAccount)
                   .WithMany(x => x.ScheduledTransfers)
                   .HasForeignKey(x => x.FromAccountId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
