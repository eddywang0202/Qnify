using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class CasePropertyRepository : BaseRepository
    {
        public CasePropertyRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<CaseProperty> GetCaseProperties()
        {
            const string commandText =
@"SELECT `id`,
`name`,
`value`
FROM `case_property`";

            var result = Conn.Query<CaseProperty>(commandText).ToList();
            return result;
        }
    }
}
