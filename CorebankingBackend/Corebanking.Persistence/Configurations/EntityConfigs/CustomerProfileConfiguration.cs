using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class CustomerProfileConfiguration : IEntityTypeConfiguration<CustomerProfile>
    {
        public void Configure(EntityTypeBuilder<CustomerProfile> builder)
        {
            builder.HasOne<AppIdentityUser>()
                   .WithOne()
                   .HasForeignKey<CustomerProfile>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AppIdentityUser>()
                   .WithMany()
                   .HasForeignKey(x => x.KycVerifiedBy)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
