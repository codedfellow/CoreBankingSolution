using Corebanking.Domain.Common;
using Corebanking.Domain.Entities;
using Corebanking.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Corebanking.Persistence.Data
{
    public class BankingDbContext(DbContextOptions<BankingDbContext> options)
    : IdentityDbContext<AppIdentityUser, AppIdentityRole, Guid>(options)
    {


        // Banking entities

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        //public DbSet<Account> Accounts
        //    => Set<Account>();


        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(
                typeof(BankingDbContext).Assembly);


            ConfigureIdentityTables(builder);
        }


        private static void ConfigureIdentityTables(
            ModelBuilder builder)
        {
            builder.Entity<AppIdentityUser>()
                .ToTable("Users");


            builder.Entity<AppIdentityRole>()
                .ToTable("Roles");


            builder.Entity<IdentityUserRole<Guid>>()
                .ToTable("UserRoles");


            builder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("UserClaims");


            builder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("UserLogins");


            builder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("RoleClaims");


            builder.Entity<IdentityUserToken<Guid>>()
                .ToTable("UserTokens");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.ModifiedAtUtc = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
