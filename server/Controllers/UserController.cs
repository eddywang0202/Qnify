using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Response;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using System.Collections.Generic;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDemographicQuestionService _demographicQuestionService;

        public UserController(IDemographicQuestionService demographicQuestionService)
        {
            _demographicQuestionService = demographicQuestionService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("Demographic/{testId}/{userId}")]
        public ActionResult<UserDemographicModelResponse> GetUserDemographicAnswers(uint testId, uint userId)
        {
            if (testId == 0 || userId == 0) return BadRequest("Test id or User id cannot be null");
            return _demographicQuestionService.GetUserDemographicAnswers(testId, userId);
        }
    }
}
