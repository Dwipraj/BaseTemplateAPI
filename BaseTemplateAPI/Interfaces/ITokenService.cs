using BaseTemplateAPI.Entity.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Interfaces
{
    interface ITokenService
    {
        string CreateToken(AppUser user, string role = null);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
