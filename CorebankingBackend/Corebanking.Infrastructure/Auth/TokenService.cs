using Corebanking.Application.Contracts.Common;
using Corebanking.Application.DTOs;
using Corebanking.Infrastructure.Configurations;
using Corebanking.Persistence.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Corebanking.Infrastructure.Auth
{
    public sealed class TokenService() : ITokenService
    {
        public (string token, DateTime expiresAtUtc) GenerateAccessToken(
        Guid userId, string email, string firstName, string lastName, IList<string> roles)
        {
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(EnvironmentVariables.AccessTokenMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("first_name", firstName),
            new("last_name", lastName)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: EnvironmentVariables.JwtIssuer,
                audience: EnvironmentVariables.JwtAudience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
        }

        public (string token, DateTime expiresAtUtc) GenerateAccessToken(AppUserDto user, IList<string> roles)
        {
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(EnvironmentVariables.AccessTokenMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("first_name", user.FirstName),
            new("last_name", user.LastName)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: EnvironmentVariables.JwtIssuer,
                audience: EnvironmentVariables.JwtAudience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
