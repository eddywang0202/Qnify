using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Qnify.Utility
{
    public static class JwtSecurityTokenExtensions
    {
        public static Guid GetUserId(this JwtSecurityToken jwtTokens)
        {
            Guid userId;
            var sid = jwtTokens.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Guid.TryParse(sid, out userId);
            return userId;
        }

        public static string GetRole(this JwtSecurityToken jwtTokens)
        {
            string role = jwtTokens.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            return role;
        }

    }
}
