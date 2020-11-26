using Microsoft.Extensions.Configuration;
using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;
using System.Linq;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Service
{
    public class UserTestQuestionAnswerService : IUserTestQuestionAnswerService
    {
        private static Database _database;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;
        private readonly ITestSetService _testSetService;

        public UserTestQuestionAnswerService(IUserService userService, ITokenService tokenService, IUserProfileService userProfileService,
            IConfiguration configuration, ITestSetService testSetService)
        {
            _database = new Database(Config.AppSettings);
            _userService = userService;
            _tokenService = tokenService;
            _userProfileService = userProfileService;
            _configuration = configuration;
            _testSetService = testSetService;
        }

        //public (string accessToken, string errorMessage) InsertUserTestQuestionAnswer(UserQuestionAnswerRequest request)
        //{            
        //    var result = false;
        //    string accessToken = "";

        //    using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
        //    {
        //        var username = request.UserQuestionAnswers.Where(x => x.TestQuestionId == 1).FirstOrDefault().ChoosenAnswer;
        //        var age = request.UserQuestionAnswers.Where(x => x.TestQuestionId == 2).FirstOrDefault().ChoosenAnswer;
        //        var email = request.UserQuestionAnswers.Where(x => x.TestQuestionId == 4).FirstOrDefault().ChoosenAnswer;

        //        var easyToken = _tokenService.GetTokenInfo(request.EasyToken);
        //        var resultValidateToken = ValidateToken(easyToken);

        //        if(!resultValidateToken.validation) return (null, resultValidateToken.errorMessage);

        //        var userId = _userService.InsertUser(username.Trim());
        //        var resultUpdateTokenUser = _tokenService.UpdateTokenUser(request.EasyToken, userId);
        //        var resultInsertUserProfile = _userProfileService.InsertUserProfile(username, age, email, userId);

        //        //Get test id
        //        var testId = 

        //        foreach (var userQuestionAnswer in request.UserQuestionAnswers)
        //        {
        //            result = new UserTestQuestionAnswerRepository(unitOfWork).InsertOrUpodateUserTestQuestionAnswer(
        //                userQuestionAnswer.TestQuestionId, userQuestionAnswer.ChoosenAnswer, userId);
        //        }

        //        if(result)
        //            accessToken = TokenHelper.GenerateAccessToken(username, DefaultValue.Role, _configuration["Token:Secret"], _configuration["Token:Issuer"], userId);
        //    }
        //    return (accessToken, null);
        //}

        #region Private Method

        //private (bool validation, string errorMessage) ValidateToken(EasyTokenModel easyToken)
        //{
        //    return easyToken != null ? easyToken.Username == null ? (true, "") : (false, "Duplicate username") : (false, "Invalid Token");
        //}

        #endregion
    }
}
