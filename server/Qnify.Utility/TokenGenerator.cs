using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Qnify.Utility
{
    public class TokenGenerator
    {
        public static class TokenHelper
        {
            public static string GenerateAccessToken(string username, string role, string secret, string issuer, uint userId = 0)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);
                var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
                var claims = new[]
                {
                    new Claim("name", username),
                    new Claim("role", role),
                    new Claim("userId", userId.ToString()),
                };

                var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

                return tokenHandler.WriteToken(token);
            }

            public static uint GetAccessTokenUserId(string hearderAccessToken)
            {
                var accessToken = hearderAccessToken.Replace("Bearer", "").Replace(" ","");

                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
                var userId = tokenS.Claims.First(claim => claim.Type == "userId").Value;
                return string.IsNullOrWhiteSpace(userId) ? 0 : uint.Parse(userId);
            }
        }
    }
}
