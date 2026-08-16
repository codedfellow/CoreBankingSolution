using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.HasOne(x => x.CustomerProfile)
                   .WithMany(x => x.CustomerAddresses)
                   .HasForeignKey(x => x.CustomerId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
