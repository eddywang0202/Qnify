using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Response;
using Qnify.Service.Interface;
using Qnify.Utility;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger,
            IConfiguration configuration, IAuthService authService)
        {
            _logger = logger;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthValidateRequest request)
        {
            try
            {
                var user = _authService.ValidateCredential(request.Username, request.Password);

                if (user != null)
                {
                    var accessToken = TokenHelper.GenerateAccessToken(user.Username, user.Role, _configuration["Token:Secret"], _configuration["Token:Issuer"]);
                    return Ok(new AuthValidateResponse
                    {
                        AccessToken = accessToken
                    });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
