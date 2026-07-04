using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Identity
{
    public sealed class AppIdentityRole : IdentityRole<Guid>
    {
        public AppIdentityRole() { }
        public AppIdentityRole(string roleName) : base(roleName) { }
    }
}
