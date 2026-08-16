using Corebanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasOne(x => x.Account)
                   .WithMany(x => x.Cards)
                   .HasForeignKey(x => x.AccountId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CustomerProfile)
                   .WithMany(x => x.Cards)
                   .HasForeignKey(x => x.CustomerId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
