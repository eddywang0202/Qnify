using Qnify.Model;
using Qnify.Model.Table;
using System;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface ITokenService
    {
        EasyTokenModel GenerateToken();
        UserAccessToken GetToken(Guid userId, string tokenValue);
        void RemoveToken(Guid userId, string tokenValue);
        bool UpdateTokenUser(string token, uint userId);
        List<EasyTokenModel> GetToken();
        EasyTokenModel GenerateToken(uint userId);
        (string accessToken, string errorMessage) ValidateToken(string token);
        EasyTokenModel GetTokenInfo(string token);
        bool RemoveToken(string token);
        EasyTokenModel ExtentTokenValidTime(string token);
    }
}
