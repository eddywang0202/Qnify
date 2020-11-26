using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;
using Qnify.Model.Table;

namespace Qnify.DAL
{
    public class AnswerRepository : BaseRepository
    {
        public AnswerRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public IEnumerable<Answer> GetAnswers()
        {
            const string commandText =
@"SELECT
`id` as AnswerId,
`title` as AnswerTitle,
`order` as AnswerOrder
FROM `answer`";

            var result = Conn.Query<Answer>(commandText).ToList();
            return result;
        }

        public IEnumerable<Answer> GetAnswer(List<uint> answerIds)
        {
            const string commandText =
@"SELECT
`id` as AnswerId,
`title` as AnswerTitle,
`order` as AnswerOrder
FROM `answer`
WHERE id in @AnswerIds";

            var param = new DynamicParameters();
            param.Add("@AnswerIds", answerIds);
            var result = Conn.Query<Answer>(commandText, param).ToList();
            return result;
        }

        public IEnumerable<Answer> GetAnswer(List<string> answerTitles)
        {
            const string commandText =
@"SELECT
`id` as AnswerId,
`title` as AnswerTitle,
`order` as AnswerOrder
FROM `answer`
WHERE title in @AnswerTitles";

            var param = new DynamicParameters();
            param.Add("@AnswerTitles", answerTitles);
            var result = Conn.Query<Answer>(commandText, param).ToList();
            return result;
        }
    }
}
