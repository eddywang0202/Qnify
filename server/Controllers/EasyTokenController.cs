using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qnify.Model.Request;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qnify.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EasyTokenController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurationService;
        private readonly ITokenService _tokenService;

        public EasyTokenController(ILogger<EasyTokenController> logger,
           IConfiguration configurationService, ITokenService tokenService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof( tokenService));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Generate")]
        public ActionResult<EasyTokenModel> Generate()
        {
            return _tokenService.GenerateToken();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Extend")]
        public ActionResult<EasyTokenModel> Extend([FromBody] string token)
        {
            var result = new EasyTokenModel();

            result = _tokenService.ExtentTokenValidTime(token);

            if(result.EasyTokenValue == null) return Unauthorized("Invalid Token");

            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Get")]
        public ActionResult<List<EasyTokenModel>> Get()
        {
            return _tokenService.GetToken();
        }
        
        [HttpPost("Validate")]
        public ActionResult<string> Validate([FromBody] string token)
        {
            var responseToken = _tokenService.ValidateToken(token);

            if (responseToken.accessToken == "")
                return Unauthorized(responseToken.errorMessage);
            else
                return responseToken.accessToken;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Remove")]
        public ActionResult<bool> Remove([FromBody] string token)
        {
            var result = _tokenService.RemoveToken(token);

            if (!result) return Unauthorized("Invalid Token");

            return result;
        }
    }
}
