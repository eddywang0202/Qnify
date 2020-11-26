using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using System.Collections.Generic;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionAnswerService _questionAnswerService;
        private readonly IDemographicQuestionService _demographicQuestionService;

        public QuestionController(IQuestionService questionService, IQuestionAnswerService questionAnswerService, IDemographicQuestionService demographicQuestionService)
        {
            _questionService = questionService;
            _questionAnswerService = questionAnswerService;
            _demographicQuestionService = demographicQuestionService;
        }

        #region Demographic

        [Authorize(Roles = "guest")]
        [HttpGet("Demographic")]
        public ActionResult<List<QuestionModel>> GetDemographicQuestion()
        {            
            return _demographicQuestionService.GetDemographicQuestions();
        }

        [Authorize(Roles = "guest")]
        [HttpPost("Demographic/Insert")]
        public ActionResult<string> Insert([FromBody] DemographicQuestionAnswerRequest request)
        {            
            var result = _demographicQuestionService.InsertUserDemographicQuestionAnswer(request);
            if (result.errorMessage != null) return BadRequest(result.errorMessage);
            return result.accessToken;
        }

        #endregion

        #region QuestionTemplate

        [Authorize(Roles = "admin")]
        [HttpGet("QuestionTemplate")]
        public ActionResult<TestSetModel> GetQuestionTemplate()
        {
            return _questionAnswerService.GetQuestionsAnswers();
        }

        #endregion

        #region TestQuestionList

        [Authorize(Roles = "admin, member")]
        [HttpGet("TestQuestionList/{testId}")]
        public ActionResult<List<TestSetListResponse>> GetTestQuestionList(uint testId)
        {
            if (testId == 0) return BadRequest("Test id cannot be null");

            uint userId = 0;

            if (HttpContext.Request.Headers["Authorization"].ToString() != "")
            {
                var hearder = HttpContext.Request.Headers["Authorization"].ToString();
                userId = TokenHelper.GetAccessTokenUserId(hearder);
            }

            var result = _questionService.GetTestQuestionList(testId, userId);
            return result;
        }

        [Authorize(Roles = "admin, member")]
        [HttpGet("ActiveTestQuestionList")]
        public ActionResult<List<TestSetListResponse>> GetActiveTestQuestionList()
        {
            uint userId = 0;

            if (HttpContext.Request.Headers["Authorization"].ToString() != "")
            {
                var hearder = HttpContext.Request.Headers["Authorization"].ToString();
                userId = TokenHelper.GetAccessTokenUserId(hearder);
            }

            var result = _questionService.GetActiveTestQuestionList(userId);
            return result;
        }

        #endregion

        #region TestQuestion

        [Authorize(Roles = "admin")]
        [HttpGet("TestQuestion/{testSetId}")]
        public ActionResult<TestSetModel> Get(uint testSetId)
        {
            var result = _questionService.GetTestQuestionResponse(testSetId);
            if (result == null || result.TestSetId == 0) return BadRequest("No record found");
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("TestQuestion/Insert")]
        public ActionResult<TestSetModel> Insert([FromBody] TestQuestionRequest request)
        {
            if (request.TestId == 0) return BadRequest("Test id cannot be null");

            return _questionService.Insert(request);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("TestQuestion/Update")]
        public ActionResult<TestSetModel> Update([FromBody] TestQuestionUpdateRequest request)
        {
            if(request.TestSetId == 0) return BadRequest("Test set id cannot be null");

            return _questionService.Update(request);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("TestQuestion/{testSetId}")]
        public ActionResult<bool> Delete(uint testSetId)
        {
            var result = _questionService.Delete(testSetId);
            if (!result) return BadRequest("No record found");
            return result;            
        }

        #endregion

        #region ParticipantTestQuestion

        [Authorize(Roles = "member")]
        [HttpGet("ParticipantTestQuestion/{testSetId}")]
        public ActionResult<TestSetModel> GetParticipantTestQuestion(uint testSetId)
        {
            var hearder = HttpContext.Request.Headers["Authorization"].ToString();

            var userId = TokenHelper.GetAccessTokenUserId(hearder);
            if (userId == 0) return BadRequest("No user id found");
            var result = _questionService.GetParticipantTestQuestion(testSetId, userId);
            if(result.TestSetId == 0) return BadRequest("No record found");
            return result;
        }

        //[Authorize(Roles = "member")]
        //[HttpPost("ParticipantTestQuestion/Insert")]
        //public ActionResult<bool> InsertUserTestQuestion([FromBody] ParticipantTestQuestionInsertRequest request)
        //{
        //    var hearder = HttpContext.Request.Headers["Authorization"].ToString();

        //    var userId = TokenHelper.GetAccessTokenUserId(hearder);
        //    if (userId == 0) return BadRequest("No user id found");
        //    var result = _questionService.InsertOrUpodateUserTestQuestionAnswer(request, userId);
        //    return result;
        //}

        [Authorize(Roles = "member")]
        [HttpPost("ParticipantTestQuestion/Update")]
        public ActionResult<bool> UpdateUserTestQuestion([FromBody] ParticipantTestQuestionInsertRequest request)
        {
            var hearder = HttpContext.Request.Headers["Authorization"].ToString();

            var userId = TokenHelper.GetAccessTokenUserId(hearder);
            if (userId == 0) return BadRequest("No user id found");
            var result = _questionService.InsertOrUpodateUserTestQuestionAnswer(request, userId);
            return result;
        }

        #endregion
    }
}
