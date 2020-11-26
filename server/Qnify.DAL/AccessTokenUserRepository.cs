using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;
using System;
using Qnify.Model.Table;

namespace Qnify.DAL
{
    public class AccessTokenUserRepository : BaseRepository
    {
        public AccessTokenUserRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public string GetToken(string tokenValue)
        {
            const string commandText =
@"SELECT
`token`
FROM `access_token_user`
where token = @Token
 LIMIT 1";

            var param = new DynamicParameters();
            param.Add("@Token", tokenValue);
            return Conn.Query<string>(commandText, param).FirstOrDefault();
        }

        public string GetToken(uint userId)
        {
            const string commandText =
@"SELECT
`token`
FROM `access_token_user`
where user_id = @UserId
 LIMIT 1";

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return Conn.Query<string>(commandText, param).FirstOrDefault();
        }

        public UserAccessToken GetToken(Guid userId, string tokenValue)
        {
            const string commandText =
@"SELECT
`user_id`,
`token`
FROM `access_token_user`
where userId = @UserId and token = @Token";

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@Token", tokenValue);
            return Conn.Query<UserAccessToken>(commandText, param).FirstOrDefault();
        }

        public List<EasyToken> GetToken()
        {
            const string commandText =
@"SELECT
u.id as UserId,
u.username as Username,
qatu.token as EasyTokenValue,
qatu.valid_datetime as Expires
FROM access_token_user qatu
left join `user` u on u.id = qatu.user_id ;";
            return Conn.Query<EasyToken>(commandText).ToList();
        }

        public EasyToken GetTokenInfo(uint userId)
        {
            const string commandText =
@"SELECT
u.id as UserId,
u.username as Username,
qatu.token as EasyTokenValue,
qatu.valid_datetime as Expires
FROM access_token_user qatu
left join `user` u on u.id = qatu.user_id
where u.id = @UserId
;";
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return Conn.Query<EasyToken>(commandText, param).FirstOrDefault();
        }

        public EasyToken GetTokenInfo(string token)
        {
            const string commandText =
@"SELECT
u.id as UserId,
u.username as Username,
qatu.token as EasyTokenValue,
qatu.valid_datetime as Expires
FROM access_token_user qatu
left join `user` u on u.id = qatu.user_id
where qatu.token = @Token
;";
            var param = new DynamicParameters();
            param.Add("@Token", token);
            return Conn.Query<EasyToken>(commandText, param).FirstOrDefault();
        }

        public bool InsertToken(string tokenValue, uint userId = 0, bool updateValidTime = false)
        {
            string updateCondition = updateValidTime ? " ON DUPLICATE KEY UPDATE `valid_datetime` = @Valid_datetime" :
                                                        " ON DUPLICATE KEY UPDATE `user_id` = @UserId";

            string commandText =
$@"INSERT INTO `access_token_user`
(`token`,`valid_datetime`)
VALUES
(@Token,@Valid_datetime)
{updateCondition};";

            var param = new DynamicParameters();
            param.Add("@Token", tokenValue);
            param.Add("@UserId", userId);
            param.Add("@Valid_datetime", DefaultValue.DatetimeTomorrow);
            return Conn.Execute(commandText, param) > 0;
        }

        public bool ExtentTokenValidTime(string tokenValue)
        {
            const string commandText =
@"UPDATE `access_token_user`
SET `valid_datetime` = @ValidDatetime
WHERE `token` = @Token;";

            var param = new DynamicParameters();
            param.Add("@Token", tokenValue);           
            param.Add("@ValidDatetime", DefaultValue.DatetimeTomorrow);
            return Conn.Execute(commandText, param) > 0;
        }

        public bool RemoveToken(string tokenValue)
        {
            const string commandText =
@"DELETE FROM `access_token_user`
where token = @Token";

            var param = new DynamicParameters();            
            param.Add("@Token", tokenValue);
            return Conn.Execute(commandText, param) > 0;
        }
    }
}
