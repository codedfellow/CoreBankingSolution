using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class KycDocumentConfiguration : IEntityTypeConfiguration<KycDocument>
    {
        public void Configure(EntityTypeBuilder<KycDocument> builder)
        {
            builder.HasOne<AppIdentityUser>()
               .WithMany()
               .HasForeignKey(x => x.ReviewedBy)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CustomerProfile)
                   .WithMany(x => x.KycDocuments)
                   .HasForeignKey(x => x.CustomerId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
