using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.FIlter
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string claimType, string claimValue) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public AuthorizeFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtTokenHeader = context.HttpContext.Request.Headers["Authorization"];
            if (JwtSecurityToken)
        }
    }
}

