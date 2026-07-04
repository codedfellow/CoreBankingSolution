using Corebanking.Application.Contracts;
using Corebanking.Application.Contracts.Common;
using Corebanking.Application.Contracts.Configurations;
using Corebanking.Infrastructure.Auth;
using Corebanking.Infrastructure.Configurations;
using Corebanking.Persistence.Data;
using Corebanking.Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Corebanking.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //Debug.WriteLine($"Connection String: {EnvironmentVariables.ConnectionString}");

            services.AddSingleton<IInfrastructureConfigrations, InfrastructureConfigrations>();

            services.AddIdentityCore<AppIdentityUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<AppIdentityRole>()
            .AddEntityFrameworkStores<BankingDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = EnvironmentVariables.JwtIssuer,
                    ValidAudience = EnvironmentVariables.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            services.AddAuthorization();
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
