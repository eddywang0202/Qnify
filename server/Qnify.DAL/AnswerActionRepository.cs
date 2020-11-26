using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class AnswerActionRepository : BaseRepository
    {
        public AnswerActionRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<AnswerAction> GetAnswerActions()
        {
            const string commandText =
@"SELECT `id`,
`value`,
`value_detail` as ValueDetail
FROM `answer_action`";

            var result = Conn.Query<AnswerAction>(commandText).ToList();
            return result;
        }
    }
}
