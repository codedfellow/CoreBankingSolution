using Corebanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class NextOfKinConfiguration : IEntityTypeConfiguration<NextOfKin>
    {
        public void Configure(EntityTypeBuilder<NextOfKin> builder)
        {
            builder.HasOne(x => x.CustomerProfile)
                   .WithMany(x => x.NextOfKins)
                   .HasForeignKey(x => x.CustomerId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
