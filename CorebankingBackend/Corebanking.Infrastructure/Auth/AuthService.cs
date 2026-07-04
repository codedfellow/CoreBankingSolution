using Corebanking.Application.Contracts;
using Corebanking.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Infrastructure.Auth
{
    public sealed class AuthService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager) : IAuthService
    {
        public async Task<AuthServiceResult> RegisterAsync(
            string firstName, string lastName, string email, string password, CancellationToken ct = default)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing is not null)
                return new AuthServiceResult(false, null, null, null, null, null,
                    ["A user with this email already exists."]);

            var user = new AppIdentityUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                return new AuthServiceResult(false, null, null, null, null, null,
                    createResult.Errors.Select(e => e.Description).ToArray());

            await userManager.AddToRoleAsync(user, "Customer");

            var roles = await userManager.GetRolesAsync(user);

            return new AuthServiceResult(true, user.Id, user.Email, user.FirstName, user.LastName, roles, null);
        }

        public async Task<AuthServiceResult> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null || !user.IsActive)
                return new AuthServiceResult(false, null, null, null, null, null, ["Invalid credentials."]);

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
                return new AuthServiceResult(false, null, null, null, null, null,
                    ["Invalid credentials."], IsLockedOut: signInResult.IsLockedOut);

            var roles = await userManager.GetRolesAsync(user);

            return new AuthServiceResult(true, user.Id, user.Email, user.FirstName, user.LastName, roles, null);
        }
    }
}
