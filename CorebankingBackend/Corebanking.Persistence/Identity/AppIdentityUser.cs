using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Persistence.Identity
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public bool IsActive { get; set; }
    }
}
