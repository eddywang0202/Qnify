using Microsoft.Extensions.Configuration;
using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Service.Mapper;
using Qnify.Utility;
using Qnify.Utility.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Service
{
    public class TokenService : ITokenService
    {
        private readonly Database _database;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public TokenService(IUserService userService, IConfiguration configuration)
        {
            _database = new Database(Config.AppSettings);
            _userService = userService;
            _configuration = configuration;
        }

        public EasyTokenModel GenerateToken()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                var generatedToken = GenerateRandomToken(unitOfWork);
                new AccessTokenUserRepository(unitOfWork).InsertToken(generatedToken);
                return GetTokenInfo(generatedToken);
            }            
        }

        public bool RemoveToken(string token)
        {
            if (!IsTokenExist(token)) return false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new AccessTokenUserRepository(unitOfWork).RemoveToken(token);
            }
        }

        public (string accessToken, string errorMessage) ValidateToken(string token)
        {
            var tokenInfo = new EasyToken();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                tokenInfo = new AccessTokenUserRepository(unitOfWork).GetTokenInfo(token);
            }

            if (tokenInfo == null) return ("", "Invalid Token");
            if (ValidateTokenExpirationWithUtc8(tokenInfo.Expires)) return ("", "Token Expired");

            var easyToken = tokenInfo.EasyTokenValue;
            var username = tokenInfo.Username;
            var userId = tokenInfo.UserId;

            return username != null ?
                (TokenHelper.GenerateAccessToken(username, DefaultValue.Role, _configuration["Token:Secret"], _configuration["Token:Issuer"], userId), "") :
                (TokenHelper.GenerateAccessToken("", DefaultValue.GuestRole, _configuration["Token:Secret"], _configuration["Token:Issuer"], userId), "");
        }

        public EasyTokenModel GetTokenInfo(string token)
        {
            var tokenInfo = new EasyToken();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                tokenInfo = new AccessTokenUserRepository(unitOfWork).GetTokenInfo(token);
            }

            return TestQuestionMapper.GenerateEasyTokenModel<EasyTokenModel>(tokenInfo);
        }

        public EasyTokenModel ExtentTokenValidTime(string token)
        {
            var tokenInfo = new EasyTokenModel();            

            if (!IsTokenExist(token)) return tokenInfo;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                new AccessTokenUserRepository(unitOfWork).ExtentTokenValidTime(token);
            }

            return GetTokenInfo(token);
        }

        public EasyTokenModel GenerateToken(uint userId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                var token = new AccessTokenUserRepository(unitOfWork).GetToken(userId);
                new AccessTokenUserRepository(unitOfWork).InsertToken(token, userId, true);
                var tokenInfo = new AccessTokenUserRepository(unitOfWork).GetTokenInfo(userId);

                return TestQuestionMapper.GenerateEasyTokenModel<EasyTokenModel>(tokenInfo);
            }
        }

        public bool UpdateTokenUser(string token, uint userId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new AccessTokenUserRepository(unitOfWork).InsertToken(token, userId);
            }
        }

        public UserAccessToken GetToken(Guid userId, string tokenValue)
        {
            var result = new UserAccessToken();
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                result = new AccessTokenUserRepository(unitOfWork).GetToken(userId, tokenValue);
            }

            return result;
        }

        public List<EasyTokenModel> GetToken()
        {
            var easyTokens = new List<EasyToken>();
            var result = new List<EasyTokenModel>();


            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                easyTokens = new AccessTokenUserRepository(unitOfWork).GetToken();

                result.AddRange(easyTokens.Select(TestQuestionMapper.GenerateEasyTokenModel<EasyTokenModel>));
            }

            return result;
        }

        public void RemoveToken(Guid userId, string tokenValue)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                new AccessTokenUserRepository(unitOfWork).GetToken(userId, tokenValue);
            }
        }

        #region Private Method

        public string GetToken(string token)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new AccessTokenUserRepository(unitOfWork).GetToken(token);
            }
        }

        private bool IsTokenExist(string token)
        {
            return GetToken(token) == null ? false : true;
        }

        private static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static bool IsTokenExist(string generatedToken, string existedToken)
        {
            return generatedToken == existedToken ? true : false;
        }

        private static string GenerateRandomToken(DapperUnitOfWork unitOfWork)
        {
            var generatedToken = RandomString(5);
            if (IsTokenExist(generatedToken, new AccessTokenUserRepository(unitOfWork).GetToken(generatedToken)))
                return GenerateRandomToken(unitOfWork);
            else return generatedToken;
        }

        private bool ValidateTokenExpirationWithUtc8(DateTime tokenValidDatetime)
        {
            return tokenValidDatetime.AddHours(8) < DateTime.UtcNow.AddHours(8) ? true : false;
        }

        #endregion
    }
}
