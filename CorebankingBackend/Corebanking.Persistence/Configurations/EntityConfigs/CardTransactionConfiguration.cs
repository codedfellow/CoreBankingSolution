using Corebanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class CardTransactionConfiguration : IEntityTypeConfiguration<CardTransaction>
    {
        public void Configure(EntityTypeBuilder<CardTransaction> builder)
        {
            builder.HasOne(x => x.Card)
                   .WithMany(x => x.CardTransactions)
                   .HasForeignKey(x => x.CardId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Transaction)
                   .WithOne(x => x.CardTransaction)
                   .HasForeignKey<CardTransaction>(x => x.TransactionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
