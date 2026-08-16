using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Configurations.EntityConfigs
{
    internal class TwoFactorCodeConfiguration : IEntityTypeConfiguration<TwoFactorCode>
    {
        public void Configure(EntityTypeBuilder<TwoFactorCode> builder)
        {
            builder.HasOne<AppIdentityUser>()
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
