using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class AccountMandateConfiguration : IEntityTypeConfiguration<AccountMandate>
    {
        public void Configure(EntityTypeBuilder<AccountMandate> builder)
        {
            builder.HasOne<AppIdentityUser>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Account)
                   .WithMany(x => x.AccountMandates)
                   .HasForeignKey(x => x.AccountId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId, x.AccountId })
               .IsUnique();
        }
    }
}
