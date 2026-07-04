using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.DTOs
{
    public record AppUserDto(Guid Id, string UserName, string Email,string FirstName, string LastName);
}
