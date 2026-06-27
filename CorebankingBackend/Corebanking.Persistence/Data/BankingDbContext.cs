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
    public class BankingDbContext : IdentityDbContext<
        ApplicationUser,
        IdentityRole<Guid>,
        Guid>
    {
        public BankingDbContext(
            DbContextOptions<BankingDbContext> options)
            : base(options)
        {
        }


        // Banking entities

        //public DbSet<Customer> Customers
        //    => Set<Customer>();


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
            builder.Entity<ApplicationUser>()
                .ToTable("Users");


            builder.Entity<IdentityRole<Guid>>()
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
    }
}
