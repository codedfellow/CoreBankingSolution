using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasOne(x => x.ToAccount)
                   .WithMany(x => x.TransfersOut)
                   .HasForeignKey(x => x.ToAccountId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Account>(x => x.FromAccount)
                   .WithMany(x => x.TransfersIn)
                   .HasForeignKey(x => x.FromAccountId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppIdentityUser>()
                   .WithMany()
                   .HasForeignKey(x => x.InitiatedBy)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppIdentityUser>()
                   .WithMany()
                   .HasForeignKey(x => x.ApprovedBy)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
