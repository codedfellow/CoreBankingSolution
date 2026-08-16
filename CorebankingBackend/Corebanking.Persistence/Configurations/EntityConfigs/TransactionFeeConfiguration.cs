using Corebanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class TransactionFeeConfiguration : IEntityTypeConfiguration<TransactionFee>
    {
        public void Configure(EntityTypeBuilder<TransactionFee> builder)
        {
            builder.HasOne(x => x.Transaction)
                   .WithMany(x => x.TransactionFees)
                   .HasForeignKey(x => x.TransactionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
